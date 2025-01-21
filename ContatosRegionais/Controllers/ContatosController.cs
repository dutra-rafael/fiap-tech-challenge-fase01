using ContatosRegionais.Aplicacao.Interfaces;
using ContatosRegionais.Aplicacao.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace ContatosRegionais.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContatosController : ControllerBase
    {
        private readonly IContatoServico _servico;
        private readonly ICacheServico _cacheServico;

        public ContatosController(IContatoServico servico, ICacheServico cacheServico)
        {
            _servico = servico;
            _cacheServico = cacheServico;
        }

        [HttpGet("ListarTodos")]
        public async Task<IActionResult> RetornarTodosAsync()
        {
            try
            {
                var key = "BuscaTodosContatos";
                var _cacheContatos = _cacheServico.Get(key);
                if (_cacheContatos != null)
                    return Ok(_cacheContatos);

                IEnumerable<ContatoModelo> _modelos = await _servico.BuscarTodosAsync();
                if (_modelos == null)
                    return NoContent();

                _cacheServico.Set(key, _modelos);

                return Ok(_modelos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("ListarPorId/{id}")]
        public async Task<IActionResult> RetornarPorIdAsync([FromRoute] int id)
        {
            try
            {
                var _cacheContato = _cacheServico.Get(id.ToString());
                if (_cacheContato != null)
                    return Ok(_cacheContato);

                ContatoModelo _modelo = await _servico.BuscarPorIdAsync(id);
                if (_modelo == null)
                    return NoContent();

                _cacheServico.Set(id.ToString(), _modelo);

                return Ok(_modelo);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("ListarPorEmail/{email}")]
        public async Task<IActionResult> RetornarPorEmailAsync([FromRoute] string email)
        {
            try
            {
                var _cacheContato = _cacheServico.Get(email);
                if (_cacheContato != null)
                    return Ok(_cacheContato);

                ContatoModelo _modelo = await _servico.BuscarPorEmailAsync(email);
                if (_modelo == null)
                    return NoContent();

                _cacheServico.Set(email.ToUpper(), _modelo);

                return Ok(_modelo);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("ListarPorNome/{nome}")]
        public async Task<IActionResult> RetornarPorNomeAsync([FromRoute] string nome)
        {
            try
            {
                var _cacheContatos = _cacheServico.Get(nome);
                if(_cacheContatos != null)
                    return Ok(_cacheContatos);

                IEnumerable<ContatoModelo> _modelos = await _servico.BuscarPorNomeAsync(nome.ToUpper());
                if (_modelos == null)
                    return NoContent();

                _cacheServico.Set(nome.ToUpper(), _modelos);

                return Ok(_modelos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("ListarPorDdd/{codigo}")]
        public async Task<IActionResult> RetornaPorCodigoAsync([FromRoute] int codigo)
        {
            try
            {
                if (codigo.ToString().Length != 2)
                    return BadRequest("O Código DDD deve possuir dois digitos.");

                var _cacheContatos = _cacheServico.Get(codigo.ToString());
                if (_cacheContatos != null)
                    return Ok(_cacheContatos);

                IEnumerable<ContatoModelo> _modelos = await _servico.BuscarPorCodigoAsync(codigo);
                if (_modelos == null)
                    return NoContent();

                _cacheServico.Set(codigo.ToString(), _modelos);

                return Ok(_modelos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("ListarPorTelefone/{codigo}&{numero}")]
        public async Task<IActionResult> RetornaPorTelefoneAsync([FromRoute] int codigo, int numero) 
        {
            try
            {
                if (codigo.ToString().Trim().Length != 2)
                    return BadRequest(new {mensagem = "O codigo de area deve ter dois digitos." });

                if(numero.ToString().Trim().Length < 8 && numero.ToString().Length > 9)
                    return BadRequest(new {mensagem = "O numero do telefone deve ter entre oito e nove digitos."});

                var _cacheContatos = _cacheServico.Get($"{codigo}{numero}");
                if(_cacheContatos != null)
                    return Ok(_cacheContatos);

                IEnumerable<ContatoModelo> _modelos = await _servico.BuscarPorTelefoneAsync(codigo, numero);
                if (!_modelos.Any())
                    return NoContent();

                _cacheServico.Set($"{codigo}{numero}", _modelos);

                return Ok(_modelos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarAsync([FromBody] ContatoNovoModelo modelo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                foreach (var telefone in modelo.Telefones)
                {
                    if (telefone.Codigo.ToString().Count() != 2)
                        return BadRequest(new { mensagem = $"O ddd deve ter 2 digitos.", telefone});

                    if (telefone.Numero.ToString().Count() < 8 || telefone.Numero.ToString().Count() > 9)
                        return BadRequest(new { mensagem = $"O telefone deve ter entre 8 e 9 digitos.", telefone});
                }

                ContatoModelo _modelo = await _servico.RegistrarAsync(modelo);
                if (modelo == null)
                    return NoContent();

                return Ok(_modelo);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarAsync([FromBody] ContatoAlteradoModelo modelo)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest();

                ContatoModelo _modelo = await _servico.AtualizarAsync(modelo);

                return Ok(_modelo);    
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ApagarAsync([FromRoute] int id)
        {
            try
            {
                if(!await _servico.RemoverAsync(id))
                    return NoContent();

                return Ok(new {mensagem = "O registro foi removido com sucesso."});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("RemoverTelefone/{id}&{codigo}&{numero}")]
        public async Task<IActionResult> ApagarTelefoneAsync([FromRoute] int id, int codigo, int numero)
        {
            try
            {
                ContatoModelo _modelo = await _servico.RemoverTelefoneAsync(id, codigo, numero);
                if (_modelo == null)
                    return BadRequest(new {mensagem= "Não foi possivel realizar a exclusão do registro. Verifique as informações e tente novamente." });

                return Ok(_modelo);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
