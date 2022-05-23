using DevTrackR.API.Entities;
using DevTrackR.API.Models;
using DevTrackR.API.Persistence.Repository;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DevTrackR.API.Controllers
{
    [Route("api/packages")]
    [ApiController]
    public class PackagesController : ControllerBase
    {  
        private readonly IPackageRepository _repository;
        private readonly ISendGridClient _client;
        public PackagesController(IPackageRepository repository, ISendGridClient client ) {
            _repository = repository;
            _client = client;
        }
        //GET api/packages
        [HttpGet]
        public IActionResult GetAll() {
            var packages = _repository.GetAll();
            return Ok(packages);
        }
        
        //GET api/packages/packageCode
        [HttpGet("{code}")]
        public IActionResult GetByCode(string code){
            var package = _repository.GetByCode(code);
            if (package == null) {
                return NotFound();
            }
            return Ok(package);
        }

        //POST api/packages
        /// <summary>
        /// Método para cadastro de um pacote
        /// </summary>
        /// <remarks>
        /// { 
        ///"title": "Pacote de teste para funcionar",
        ///"weight": 2,
        ///"senderName": "Thiago",
        ///"senderEmail": "hohar58058@dufeed.com"
        ///}
        /// </remarks>
        /// <param name="model">Aqui temos dados do pacote</param>
        /// <returns>Objeto criado</returns>
        [HttpPost]

        public async Task<IActionResult> Post(AddPackageInputModel model) {
            if(model.Title.Length <= 10) {
                return BadRequest("Favor colocar titulo grande amigo");
            }
            var package = new Package(model.Title, model.Weight);
            _repository.Add(package);

            var message = new SendGridMessage {
                From = new EmailAddress("hohar58058@dufeed.com", "HOHAR"),
                Subject = "Seu pacote foi enviado!",
                PlainTextContent = $"Seu pacote de código {package.Code} foi enviado!" 
            };
            message.AddTo(model.SenderEmail, model.SenderName);
            await _client.SendEmailAsync(message);
            return CreatedAtAction("GetByCode", new {code = package.Code}, package);
        }

        //POST api/packages/packageCode/updates

        [HttpPost("{code}/updates")]

        public IActionResult PostUpdate(string code, AddPackageUpdateInputModel model){

            var package = _repository.GetByCode(code);
            if (package == null) {
                return NotFound();
            }
            
            package.AddUpdate(model.Status, model.Delivered);
            _repository.Update(package);
            return NoContent();
        }
    }
}