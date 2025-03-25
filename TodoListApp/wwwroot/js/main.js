import { createTodoItem, getTodoItems, updateTodoItem, deleteTodoItem } from './todoService.js'; // Ensure the correct path

document.addEventListener('DOMContentLoaded', function() {
  console.log('Main script loaded');
  // Ensure the DOM is fully loaded before accessing elements
  const createTodoForm = document.getElementById('create-todo-form');
  if (createTodoForm) {
    // Example: Create a new todo item
    createTodoForm.addEventListener('submit', async (event) => {
      event.preventDefault();
      console.log('Create todo form submitted');
      const title = event.target.elements['title'].value;
      try {
        console.log('Calling createTodoItem');
        await createTodoItem(title);
        // Refresh the todo list
        loadTodoItems();
      } catch (error) {
        console.error('Error creating todo item:', error);
      }
    });
  } else {
    console.error('create-todo-form element not found');
  }

  // Initial load of todo items
  loadTodoItems();
});

// Example: Load and display todo items
async function loadTodoItems() {
  console.log('Loading todo items');
  try {
    console.log('Calling getTodoItems');
    const todoItems = await getTodoItems();
    console.log('Todo items response:', todoItems); // Debugging log
    if (!todoItems || !Array.isArray(todoItems)) {
      console.error('Todo items are undefined or not an array');
      return;
    }
    // Render todo items in the UI
    const todoList = document.getElementById('todo-list');
    if (todoList) {
      todoList.innerHTML = '';
      todoItems.forEach(item => {
        const listItem = document.createElement('li');
        listItem.textContent = item.title;
        // Add a checkbox to update isCompleted status
        const checkbox = document.createElement('input');
        checkbox.type = 'checkbox';
        checkbox.checked = item.isCompleted;
        checkbox.style.marginRight = '10px'; // Add margin between checkbox and title
        checkbox.addEventListener('change', () => handleUpdateTodoItem(item.id, { isCompleted: checkbox.checked }));
        listItem.insertBefore(checkbox, listItem.firstChild); // Insert checkbox before the title
        // Add delete button
        const deleteButton = document.createElement('button');
        deleteButton.textContent = 'Delete';
        deleteButton.addEventListener('click', () => handleDeleteTodoItem(item.id));
        listItem.appendChild(deleteButton);
        todoList.appendChild(listItem);
      });
    } else {
      console.error('todo-list element not found');
    }
  } catch (error) {
    console.error('Error loading todo items:', error);
  }
}

// Example: Update a todo item
async function handleUpdateTodoItem(id, updates) {
  console.log(`[DEBUG] Updating todo item with Id=${id}, Updates=`, updates);
  try {
    console.log('[DEBUG] Calling updateTodoItem');
    await updateTodoItem(id, updates);
    console.log(`[DEBUG] Todo item with Id=${id} updated successfully.`);
    // Refresh the todo list
    loadTodoItems();
  } catch (error) {
    console.error(`[ERROR] Error updating todo item with Id=${id}:`, error);
  }
}

// Example: Delete a todo item
async function handleDeleteTodoItem(id) {
  console.log('Deleting todo item:', id);
  try {
    console.log('Calling deleteTodoItem');
    await deleteTodoItem(id);
    // Refresh the todo list
    loadTodoItems();
  } catch (error) {
    console.error('Error deleting todo item:', error);
  }
}

async function fetchTodoItems() {
    try {
        const response = await fetch('/Todo/Index', {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            console.error(`Error fetching todo items: ${response.statusText}`);
            return;
        }

        const data = await response.json();
        if (!data || !Array.isArray(data)) {
            console.error('Invalid todo items response:', data);
            return;
        }

        console.log('Todo items response:', data);
        // Process and render the todo items here
    } catch (error) {
        console.error('Error fetching todo items:', error);
    }
}

fetchTodoItems();
