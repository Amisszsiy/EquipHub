using EquipHub_API.MockupData;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EquipHub_API.Controllers
{
    [Route("api/EquipHubAPI")]
    [ApiController]
    public class EquipHubAPIController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Tool>> GetTools()
        {
            return Ok(ToolData.toolData);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Tool> GetTool(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            Tool tool = ToolData.toolData.FirstOrDefault(u => u.Id == id);
            if(tool == null)
            {
                return NotFound();
            }
            return Ok(tool);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Tool> AddTool([FromBody]Tool tool)
        {
            if(tool == null)
            {
                return BadRequest();
            }
            if(tool.Id == 0)
            {
                tool.Id = ToolData.toolData.Count() + 1;
                ToolData.toolData.Add(tool);
                return Ok(tool);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}")]
        //IActionResult because of no return type
        public IActionResult DeleteTool(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            Tool tool = ToolData.toolData.FirstOrDefault(t => t.Id == id);
            if(tool == null)
            {
                return NotFound();
            }
            ToolData.toolData.Remove(tool);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{id:int}")]
        public IActionResult UpdateTool(int id, [FromBody]Tool tool)
        {
            //If parameter is wrong, I prevent magic happen this way
            if(tool == null || tool.Id !=  id)
            {
                return BadRequest();
            }
            Tool queryTool = ToolData.toolData.FirstOrDefault(t => t.Id == id);
            queryTool.Name = tool.Name;
            queryTool.Owner = tool.Owner;
            queryTool.Note = tool.Note;

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdatePartialTool(int id, JsonPatchDocument<Tool> patchTool)
        {
            if(patchTool == null || id == 0)
            {
                return BadRequest();
            }
            Tool queryTool = ToolData.toolData.FirstOrDefault(u => u.Id == id);
            if(queryTool == null)
            {
                return NotFound();
            }
            patchTool.ApplyTo(queryTool, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
