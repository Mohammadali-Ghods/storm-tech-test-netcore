using System.Linq;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoLists;

namespace Todo.EntityModelMappers.TodoLists
{
    public static class TodoListDetailViewmodelFactory
    {
        public static TodoListDetailViewmodel Create(TodoList todoList)
        {
            var items = todoList.Items.OrderBy(x=>x.Importance).Select(TodoItemSummaryViewmodelFactory.Create).ToList();
            return new TodoListDetailViewmodel(todoList.TodoListId, todoList.Title, items);
        }
    }
}