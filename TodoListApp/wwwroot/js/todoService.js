// Remove the import statement
// import { createClient } from './node_modules/@supabase/supabase-js';

let supabase;

function initializeSupabase() {
  const supabaseUrl = 'https://ienqhzalrsvsqjvgwddq.supabase.co';
  const supabaseKey = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImllbnFoemFscnN2c3Fqdmd3ZGRxIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDExNTg3OTQsImV4cCI6MjA1NjczNDc5NH0.sTvrOOzK1VeZ1HIkRqFZ0AE7BUDDTCzqiS73iMcU6Zo';
  supabase = window.supabase.createClient(supabaseUrl, supabaseKey);
}

// Validate input
function validateInput(input) {
  if (!input || typeof input !== 'string' || input.trim() === '') {
    throw new Error('Invalid input');
  }
}


// Create a new todo item
export async function createTodoItem(title) {
  console.log('Creating todo item:', title);
  validateInput(title);
  const { data, error } = await supabase
    .from('todos')
    .insert([{ title, isCompleted: false }]);
  if (error) {
    console.error('Error creating todo item:', error);
    throw error;
  }
  console.log('Created todo item:', data);
  return data;
}

// Read all todo items
export async function getTodoItems() {
  console.log('Fetching todo items');
  const { data, error } = await supabase
    .from('todos')
    .select('*');
  if (error) {
    console.error('Error fetching todo items:', error);
    throw error;
  }
  console.log('Fetched todo items:', data);
  if (data.length === 0) {
    console.log('No todo items found');
  } else {
    data.forEach(item => {
      console.log('Todo item:', item);
    });
  }
  return data;
}

// Update a todo item
export async function updateTodoItem(id, updates) {
  console.log('Updating todo item:', id, updates);
  if (!id || typeof id !== 'number') {
    throw new Error('Invalid ID');
  }
  if (updates.title) {
    validateInput(updates.title);
  }
  const { data, error } = await supabase
    .from('todos')
    .update(updates)
    .eq('id', id);
  if (error) {
    console.error('Error updating todo item:', error);
    throw error;
  }
  console.log('Updated todo item:', data);
  return data;
}



// Delete a todo item
export async function deleteTodoItem(id) {
  console.log('Deleting todo item:', id);
  if (!id || typeof id !== 'number') {
    throw new Error('Invalid ID');
  }
  const { data, error } = await supabase
    .from('todos')
    .delete()
    .eq('id', id);
  if (error) {
    console.error('Error deleting todo item:', error);
    throw error;
  }
  console.log('Deleted todo item:', data);
  return data;
}
