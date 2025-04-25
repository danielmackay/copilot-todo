import { useState, useEffect } from 'react';
import TodoForm from './components/TodoForm';
import TodoList from './components/TodoList';
import { Todo } from './types';
import './App.css';

const API_URL = '/api';  // Using proxy configured in vite.config.ts

function App() {
  const [todos, setTodos] = useState<Todo[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  // Fetch todos from API
  useEffect(() => {
    const fetchTodos = async () => {
      try {
        setLoading(true);
        const response = await fetch(`${API_URL}/todos`);
        if (!response.ok) {
          throw new Error('Failed to fetch todos');
        }
        const data = await response.json();
        setTodos(data);
        setError(null);
      } catch (err) {
        console.error('Error fetching todos:', err);
        setError('Failed to load tasks. Using local state only.');
        // Fallback to empty list if API is unavailable
      } finally {
        setLoading(false);
      }
    };

    fetchTodos();
  }, []);

  // Add a new todo
  const addTodo = async (title: string) => {
    try {
      const response = await fetch(`${API_URL}/todos`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ title }),
      });

      if (!response.ok) {
        throw new Error('Failed to add todo');
      }

      const newTodo = await response.json();
      setTodos([...todos, newTodo]);
    } catch (err) {
      console.error('Error adding todo:', err);
      // Fallback to local state
      const newTodo: Todo = {
        id: crypto.randomUUID(),
        title,
        isCompleted: false,
        createdAt: new Date().toISOString(),
        completedAt: null,
      };
      setTodos([...todos, newTodo]);
    }
  };

  // Toggle todo completion status
  const toggleTodoComplete = async (id: string) => {
    const todoToUpdate = todos.find(todo => todo.id === id);
    if (!todoToUpdate) return;

    try {
      const response = await fetch(`${API_URL}/todos/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          isCompleted: !todoToUpdate.isCompleted,
        }),
      });

      if (!response.ok) {
        throw new Error('Failed to update todo');
      }

      const updatedTodo = await response.json();
      setTodos(
        todos.map(todo => (todo.id === id ? updatedTodo : todo))
      );
    } catch (err) {
      console.error('Error updating todo:', err);
      // Fallback to local state
      setTodos(
        todos.map(todo =>
          todo.id === id
            ? {
                ...todo,
                isCompleted: !todo.isCompleted,
                completedAt: !todo.isCompleted ? new Date().toISOString() : null,
              }
            : todo
        )
      );
    }
  };

  // Delete a todo
  const deleteTodo = async (id: string) => {
    try {
      const response = await fetch(`${API_URL}/todos/${id}`, {
        method: 'DELETE',
      });

      if (!response.ok) {
        throw new Error('Failed to delete todo');
      }

      setTodos(todos.filter(todo => todo.id !== id));
    } catch (err) {
      console.error('Error deleting todo:', err);
      // Fallback to local state
      setTodos(todos.filter(todo => todo.id !== id));
    }
  };

  return (
    <div className="container max-w-xl p-8 mx-auto mt-10 rounded-lg shadow-lg">
      <h1 className="mb-8 text-3xl font-bold text-center text-white">Todo App</h1>
      
      {error && (
        <div className="p-4 mb-6 text-sm text-yellow-800 bg-yellow-100 rounded-lg dark:bg-yellow-900 dark:text-yellow-200">
          {error}
        </div>
      )}
      
      <TodoForm addTodo={addTodo} />
      
      {loading ? (
        <div className="flex justify-center p-6">
          <div className="w-8 h-8 border-t-2 border-b-2 border-blue-500 rounded-full animate-spin"></div>
        </div>
      ) : (
        <TodoList
          todos={todos}
          onToggleComplete={toggleTodoComplete}
          onDelete={deleteTodo}
        />
      )}
    </div>
  );
}

export default App;
