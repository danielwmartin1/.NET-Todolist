import { createTodoItem, getTodoItems, updateTodoItem, deleteTodoItem } from './todoService';

console.log('Main script loaded');

/* ...existing code... */

// Example: Create a new todo item
document.getElementById('create-todo-form').addEventListener('submit', async (event) => {
  event.preventDefault();
  console.log('Create todo form submitted');
  const title = event.target.elements['title'].value;
  try {
    await createTodoItem(title);
    // Refresh the todo list
    loadTodoItems();
  } catch (error) {
    console.error('Error creating todo item:', error);
  }
});

// Example: Load and display todo items
async function loadTodoItems() {
  console.log('Loading todo items');
  try {
    const todoItems = await getTodoItems();
    console.log('Todo items:', todoItems);
    // Render todo items in the UI
    /* ...existing code to render todo items... */
  } catch (error) {
    console.error('Error loading todo items:', error);
  }
}

// Example: Update a todo item
async function handleUpdateTodoItem(id, updates) {
  console.log('Updating todo item:', id, updates);
  try {
    await updateTodoItem(id, updates);
    // Refresh the todo list
    loadTodoItems();
  } catch (error) {
    console.error('Error updating todo item:', error);
  }
}

// Example: Delete a todo item
async function handleDeleteTodoItem(id) {
  console.log('Deleting todo item:', id);
  try {
    await deleteTodoItem(id);
    // Refresh the todo list
    loadTodoItems();
  } catch (error) {
    console.error('Error deleting todo item:', error);
  }
}

/* ...existing code... */
