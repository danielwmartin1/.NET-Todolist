// ...existing code...

let currentlyEditingId = null; // Track the currently open edit form

function openEditForm(todoId) {
  // Prevent opening another edit form if the same one is already open
  if (currentlyEditingId === todoId) {
    return;
  }

  // Close all other edit forms
  closeAllEditForms();

  // Open the new edit form
  const editForm = document.getElementById(`edit-form-${todoId}`);
  if (editForm) {
    editForm.style.display = 'block';
    currentlyEditingId = todoId;

    // Add a click listener to detect clicks outside the edit input
    document.addEventListener('click', handleOutsideClick);
  }
}

function closeAllEditForms() {
  // Close all edit forms and reset the state
  document.querySelectorAll('.edit-form').forEach(form => {
    form.style.display = 'none';
  });
  currentlyEditingId = null;

  // Remove the click listener to avoid unnecessary triggers
  document.removeEventListener('click', handleOutsideClick);
}

function closeEditForm(todoId) {
  const editForm = document.getElementById(`edit-form-${todoId}`);
  if (editForm) {
    editForm.style.display = 'none';
    currentlyEditingId = null;

    // Remove the click listener when the edit form is closed
    document.removeEventListener('click', handleOutsideClick);
  }
}

function handleOutsideClick(event) {
  const listItem = document.querySelector(`.list-group-item[data-todo-id="${currentlyEditingId}"]`);
  if (listItem && !listItem.contains(event.target)) {
    // Close the edit form if the click is outside the list-group-item
    closeEditForm(currentlyEditingId);
  }
}

// Attach event listeners to edit and cancel buttons dynamically
function attachEventListeners() {
  document.querySelectorAll('.edit-button').forEach(button => {
    button.addEventListener('click', event => {
      event.stopPropagation(); // Prevent triggering the outside click handler
      const todoId = event.target.dataset.todoId;
      openEditForm(todoId);
    });
  });

  document.querySelectorAll('.cancel-button').forEach(button => {
    button.addEventListener('click', event => {
      event.stopPropagation(); // Prevent triggering the outside click handler
      const todoId = event.target.dataset.todoId;
      closeEditForm(todoId);
    });
  });
}

// Call attachEventListeners after rendering the todo list
function renderTodoList(todoItems) {
  // Sort todo items by createdAt (most recent first)
  todoItems.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));

  const todoListContainer = document.getElementById('todo-list');
  todoListContainer.innerHTML = ''; // Clear existing items

  todoItems.forEach(item => {
    const todoElement = document.createElement('div');
    todoElement.className = 'list-group-item todo-item'; // Add list-group-item class
    todoElement.setAttribute('data-todo-id', item.id); // Add data-todo-id for identification
    todoElement.innerHTML = `
      <span>${item.title} - ${new Date(item.createdAt).toLocaleString()}</span>
      <button class="edit-button" data-todo-id="${item.id}">Edit</button>
      <div id="edit-form-${item.id}" class="edit-form" style="display: none;">
        <input type="text" value="${item.title}" />
        <button class="cancel-button" data-todo-id="${item.id}">Cancel</button>
      </div>
    `;
    todoListContainer.appendChild(todoElement);
  });

  // Reattach event listeners after rendering
  attachEventListeners();
}

// Example usage
fetch('/api/todos')
  .then(response => response.json())
  .then(data => renderTodoList(data))
  .catch(error => console.error('Error fetching todo items:', error));

// ...existing code...