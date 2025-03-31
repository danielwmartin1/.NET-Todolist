// Track the currently open edit form
let currentlyEditingId = null;

// Open the edit form for a specific todo item
function openEditForm(todoId) {
  // Prevent reopening the same edit form
  if (currentlyEditingId === todoId) return;

  // Close any other open edit forms
  closeAllEditForms();

  // Display the edit form for the selected todo item
  const editForm = document.getElementById(`edit-form-${todoId}`);
  if (editForm) {
    editForm.style.display = 'block';
    currentlyEditingId = todoId;

    // Add a listener to detect clicks outside the edit form
    document.addEventListener('click', handleOutsideClick);
  }
}

// Close all open edit forms
function closeAllEditForms() {
  document.querySelectorAll('.edit-form').forEach(form => {
    form.style.display = 'none';
  });

  currentlyEditingId = null;

  // Remove the outside click listener
  document.removeEventListener('click', handleOutsideClick);
}

// Close the edit form for a specific todo item
function closeEditForm(todoId) {
  const editForm = document.getElementById(`edit-form-${todoId}`);
  if (editForm) {
    editForm.style.display = 'none';
    currentlyEditingId = null;

    // Remove the outside click listener
    document.removeEventListener('click', handleOutsideClick);
  }
}

// Handle clicks outside the currently open edit form
function handleOutsideClick(event) {
  const listItem = document.querySelector(`.list-group-item[data-todo-id="${currentlyEditingId}"]`);
  if (listItem && !listItem.contains(event.target)) {
    closeEditForm(currentlyEditingId);
  }
}

// Attach event listeners to edit and cancel buttons
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

// Render the todo list dynamically
function renderTodoList(todoItems) {
  // Sort todo items by creation date (most recent first)
  todoItems.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));

  const todoListContainer = document.getElementById('todo-list');
  todoListContainer.innerHTML = ''; // Clear existing items

  // Create and append todo items
  todoItems.forEach(item => {
    const todoElement = document.createElement('div');
    todoElement.className = 'list-group-item todo-item';
    todoElement.setAttribute('data-todo-id', item.id);
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

// Fetch and render the todo list
fetch('/api/todos')
  .then(response => response.json())
  .then(data => renderTodoList(data))
  .catch(error => console.error('Error fetching todo items:', error));
