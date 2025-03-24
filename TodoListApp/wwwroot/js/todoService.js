// Removed the import statement
// import { createClient } from './node_modules/@supabase/supabase-js';

let supabase;

function initializeSupabase() {
  console.log('Initializing Supabase...');
  if (!supabase) {
    if (!window.supabase) {
      console.error('Supabase library is not loaded.');
      return;
    }
    const supabaseUrl = process.env.SUPABASE_URL;
    const supabaseKey = process.env.SUPABASE_KEY;
    if (!supabaseUrl || !supabaseKey) {
      console.error('Supabase URL or Key is not set in environment variables.');
      return;
    }
    supabase = window.supabase.createClient(supabaseUrl, supabaseKey);
    console.log('Supabase initialized:', supabase);
  }
}

// Validate input
function validateInput(input) {
  console.log('Validating input:', input);
  if (!input || typeof input !== 'string' || input.trim() === '') {
    throw new Error('Invalid input');
  }
}

// Create a new todo item
export async function createTodoItem(title) {
  console.log('createTodoItem called with title:', title);
  initializeSupabase();
  if (!supabase) return;
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
  console.log('getTodoItems called');
  initializeSupabase();
  if (!supabase) return;
  const { data, error } = await supabase
    .from('todos')
    .select('*');
  console.log('Supabase response:', { data, error });
  if (error) {
    console.error('Error fetching todo items:', error);
    throw error;
  }
  console.log('Fetched todo items:', data);
  return data;
}

// Update a todo item
export async function updateTodoItem(id, updates) {
  console.log('updateTodoItem called with id:', id, 'updates:', updates);
  initializeSupabase();
  if (!supabase) return;
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
  console.log('deleteTodoItem called with id:', id);
  initializeSupabase();
  if (!supabase) return;
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
