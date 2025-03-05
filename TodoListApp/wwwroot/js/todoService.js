import { supabase } from './supabaseClient';

// Create a new todo item
export async function createTodoItem(title) {
  const { data, error } = await supabase
    .from('todos')
    .insert([{ title }]);
  if (error) throw error;
  return data;
}

// Read all todo items
export async function getTodoItems() {
  const { data, error } = await supabase
    .from('todos')
    .select('*');
  if (error) throw error;
  return data;
}

// Update a todo item
export async function updateTodoItem(id, updates) {
  const { data, error } = await supabase
    .from('todos')
    .update(updates)
    .eq('id', id);
  if (error) throw error;
  return data;
}

// Delete a todo item
export async function deleteTodoItem(id) {
  const { data, error } = await supabase
    .from('todos')
    .delete()
    .eq('id', id);
  if (error) throw error;
  return data;
}
