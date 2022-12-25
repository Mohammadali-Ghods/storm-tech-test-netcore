using System.Collections.Generic;
using System.Linq;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoItems;
using Todo.Models.TodoLists;

namespace Todo.EntityModelMappers.TodoLists
{
    public static class TodoListDetailViewmodelFactory
    {
        public static TodoListDetailViewmodel Create(TodoList todoList, bool hideDoneItems, bool orderByRankAsc)
        {
            List<TodoItemSummaryViewmodel> items = null;

            if (!hideDoneItems)
                if (orderByRankAsc)
                    items = todoList.Items.OrderBy(x => x.Rank).Select(TodoItemSummaryViewmodelFactory.Create).ToList();
                else
                    items = todoList.Items.OrderByDescending(x => x.Rank).Select(TodoItemSummaryViewmodelFactory.Create).ToList();
            else
                  if (orderByRankAsc)
                items = todoList.Items.Where(x => x.IsDone == false).OrderBy(x => x.Rank).OrderBy(x => x.Importance).Select(TodoItemSummaryViewmodelFactory.Create).ToList();
            else
                items = todoList.Items.Where(x => x.IsDone == false).OrderByDescending(x => x.Rank).OrderBy(x => x.Importance).Select(TodoItemSummaryViewmodelFactory.Create).ToList();

            return new TodoListDetailViewmodel(todoList.TodoListId, todoList.Title, items, hideDoneItems, orderByRankAsc);

        }
        public static TodoListDetailViewmodel Create(TodoList todoList)
        {
            List<TodoItemSummaryViewmodel> items = null;
            items = todoList.Items.OrderBy(x => x.Importance).Select(TodoItemSummaryViewmodelFactory.Create).ToList();
            return new TodoListDetailViewmodel(todoList.TodoListId, todoList.Title, items, false, false);
        }
    }
}