using ApartmentManagement.AuthorizationOperations;
using ApartmentManagement.DBOperations;
using ApartmentManagement.Entities;
using ApartmentManagement.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagement.Controllers;

[Route("[controller]s")]
public class MessageController : Controller
{
    private readonly ApartmentDBContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<HomeController> _logger;

    public MessageController(ILogger<HomeController> logger, ApartmentDBContext context, IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    [RoleAttribute]
    public IActionResult Index()
    {
        var currentRole = (int)HttpContext.Items["Role"];
        List<string> emailList;
        if (currentRole == (int)UserType.admin)
            emailList = _context.Users.Select(user => user.EMail).ToList();
        else
            emailList = _context.Users.Where(user => user.Role == UserType.admin).Select(user => user.EMail).ToList();

        return View(emailList);
    }

    [RoleAttribute]
    [HttpGet("[action]")]
    public IActionResult RaiseMessage([FromQuery] string EMail)
    {
        var currentUser = HttpContext.Items["User"] as User;
        var toUser = _context.Users.FirstOrDefault(user => user.EMail == EMail);

        if (toUser == null)
            return NotFound();

        // get all messages from current user to toUser and vice versa
        var messages = _context.Messages.Where(message => (message.FromId == currentUser.Id && message.ToId == toUser.Id) || (message.FromId == toUser.Id && message.ToId == currentUser.Id)).ToList();
        var messageListViewModel = _mapper.Map<List<MessageViewModel>>(messages);
        return View(messageListViewModel);
    }

    [RoleAttribute]
    [HttpPost("[action]")]
    public IActionResult RaiseMessage([FromForm] string Content, [FromQuery] string EMail)
    {
        var currentUser = HttpContext.Items["User"] as User;
        var toUser = _context.Users.FirstOrDefault(user => user.EMail == EMail);

        if (toUser == null)
            return NotFound();

        var message = new Message
        {
            FromId = currentUser.Id,
            ToId = toUser.Id,
            Content = Content,
            Date = DateTime.Now.ToUniversalTime()
        };

        _context.Messages.Add(message);
        _context.SaveChanges();

        return RedirectToAction("RaiseMessage", new { EMail = EMail });
    }
}