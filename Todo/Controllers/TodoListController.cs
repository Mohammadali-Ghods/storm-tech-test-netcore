using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoLists;
using Todo.Interface;
using Todo.Models;
using Todo.Models.TodoLists;
using Todo.Services;

namespace Todo.Controllers
{
    [Authorize]
    public class TodoListController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IUserStore<IdentityUser> userStore;
        private readonly IGravatarAPI gravatarAPI;

        public TodoListController(ApplicationDbContext dbContext, IUserStore<IdentityUser> userStore, IGravatarAPI gravatarAPI
            )
        {
            this.dbContext = dbContext;
            this.userStore = userStore;
            this.gravatarAPI = gravatarAPI;
        }

        public IActionResult Index()
        {
            var userId = User.Id();
            var todoLists = dbContext.RelevantTodoLists(userId);
            var viewmodel = TodoListIndexViewmodelFactory.Create(todoLists);
            return View(viewmodel);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int todoListId, bool hideDoneItems, bool orderByRankAsc, bool firstOne)
        {
            var todoList = dbContext.SingleTodoList(todoListId);

            if (!firstOne)
                return View(await UpdateUsersDisplayNameAndImageWithGravatar(TodoListDetailViewmodelFactory.Create(todoList, hideDoneItems, orderByRankAsc)));
            else
                return View(await UpdateUsersDisplayNameAndImageWithGravatar(TodoListDetailViewmodelFactory.Create(todoList)));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TodoListFields());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoListFields fields)
        {
            if (!ModelState.IsValid) { return View(fields); }

            var currentUser = await userStore.FindByIdAsync(User.Id(), CancellationToken.None);

            var todoList = new TodoList(currentUser, fields.Title);

            await dbContext.AddAsync(todoList);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Create", "TodoItem", new { todoList.TodoListId });
        }

        private async Task<TodoListDetailViewmodel> UpdateUsersDisplayNameAndImageWithGravatar(
            TodoListDetailViewmodel input)
        {
            var distinctItems = input.Items.Select(x => x.ResponsibleParty).Distinct();
            List<UserModel> distinctObject = new List<UserModel>();

            foreach (var item in distinctItems)
            {
                var result = await gravatarAPI.Get(item.Email);
                if (result != null)
                    distinctObject.Add(new UserModel()
                    {
                        UserDisplayName = result.entry[0].displayName,
                        UserImage = result.entry[0].thumbnailUrl,
                        Email = item.Email
                    });
            }

            foreach (var item in input.Items)
            {
                if (distinctObject.Where
                    (x => x.Email == item.ResponsibleParty.UserName).Count() != 0)
                {
                    item.UserDetail = distinctObject.Where
                    (x => x.Email == item.ResponsibleParty.UserName).FirstOrDefault();
                }
            }

            return input;
        }
    }
}