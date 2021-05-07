using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcText;
using Microsoft.Extensions.Logging;
using TextService.Data.Repositories;
using Text = TextService.Data.Models.Text;
using TextGrpc = GrpcText.Text;

namespace TextService.Services
{
    public class TextService : TextGrpc.TextBase
    {
        private readonly ITextRepository _textRepository;

        private readonly ILogger<TextService> _logger;

        public TextService(ITextRepository textRepository, ILogger<TextService> logger)
        {
            _textRepository = textRepository;
            _logger = logger;
        }

        public override async Task<TextItemResponse> GetTextById(TextRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call from method {Method} for text id {Id}", context.Method, request.Id);
            if (Guid.TryParse(request.Id, out var id))
            {
                var text = await _textRepository.GetByIdAsync(id);
                if (text == null)
                {
                    return null;
                }
                context.Status = new Status(StatusCode.OK, $"Text with id {request.Id} do exist");
                return new TextItemResponse { Body = text.Value };
            }
            context.Status = new Status(StatusCode.InvalidArgument, "Id not guid");
            return null;
        }

        public override async Task<TextAllResponse> GetTextsAll(TextAllRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call from method {Method} for all texts", context.Method);
            var allTexts = await _textRepository.GetAsync();
            if (allTexts.Length > 0)
            {
                var response = new TextAllResponse();
                foreach (var text in allTexts)
                {
                    response.Items.Add(new TextItemResponse { Id = text.Oid.ToString(), Body = text.Value });
                }
                context.Status = new Status(StatusCode.OK, "All texts received");
                return response;
            }
            return null;
        }

        public override async Task<SaveTextResponse> SaveTextAsBinary(SaveTextAsBinaryRequest request,
            ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call from method {Method} for save text as binary", context.Method);
            var body = System.Text.Encoding.Default.GetString(request.File.ToByteArray());
            await _textRepository.CreateAsync(new Text { Value = body }, context.CancellationToken);
            await _textRepository.SaveAsync(context.CancellationToken);
            context.Status = new Status(StatusCode.OK, "Save text as binary complete");
            return new SaveTextResponse { Result = true };
        }

        public override async Task<SaveTextResponse> SaveTextAsString(SaveTextAsStringRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call from method {Method} for save text as string", context.Method);
            var body = request.Body;
            await _textRepository.CreateAsync(new Text { Value = body }, context.CancellationToken);
            await _textRepository.SaveAsync(context.CancellationToken);
            context.Status = new Status(StatusCode.OK, "Save text as string complete");
            return new SaveTextResponse { Result = true };
        }

        public override async Task<SaveTextResponse> SaveTextByFilePath(SaveTextByFilePathRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call from method {Method} for save text by uri", context.Method);
            var text = new Text();
            using (var client = new WebClient())
            {
                var filePath = $@"{request.FilePath}";
                var uri = new Uri(new Uri("file://"), filePath);
                var body = await client.DownloadStringTaskAsync(uri.AbsoluteUri);
                text.Value = body;
            }
            await _textRepository.CreateAsync(text, context.CancellationToken);
            await _textRepository.SaveAsync(context.CancellationToken);
            context.Status = new Status(StatusCode.OK, "Save text by uri complete");
            return new SaveTextResponse { Result = true };
        }
    }
}
