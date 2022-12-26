// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function AddNewItem(todolistid, title, todolisttitle, userid, importance) {
   
    const RAPIDAPI_API_URL = "/TodoItem/CreateByJs";
    const REQUEST_HEADERS = {
        'Content-Type': 'application/json'
    };
    const json = {
        todoListId: todolistid
        , title: title
        , todoListTitle: todolisttitle
        , responsiblePartyId: userid
        , importance: parseInt(importance)
    };
    console.log(json);
    axios.post(RAPIDAPI_API_URL, json, { headers: REQUEST_HEADERS })
        .then(response => {
            location.reload();
        })
        .catch(error => console.error('On create todo error', error));
}

