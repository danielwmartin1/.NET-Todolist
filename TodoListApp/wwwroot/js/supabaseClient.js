import { createClient } from '@supabase/supabase-js';

const supabaseUrl = 'https://ienqhzalrsvsqjvgwddq.supabase.co';
const supabaseKey = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImllbnFoemFscnN2c3Fqdmd3ZGRxIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDExNTg3OTQsImV4cCI6MjA1NjczNDc5NH0.sTvrOOzK1VeZ1HIkRqFZ0AE7BUDDTCzqiS73iMcU6Zo';
export const supabase = createClient(supabaseUrl, supabaseKey);
