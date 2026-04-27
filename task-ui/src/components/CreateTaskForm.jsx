import { useState, useEffect } from 'react';
import { PlusCircle, Loader2 } from 'lucide-react';
import { api } from '../api';

export default function CreateTaskForm({ onSubmit }) {
  const [title, setTitle] = useState('');
  const [desc, setDesc] = useState('');
  const [userId, setUserId] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  
  const [assignableUsers, setAssignableUsers] = useState([]);
  const [loadingUsers, setLoadingUsers] = useState(true);

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const users = await api.getAssignableUsers();
        setAssignableUsers(users);
        if (users.length > 0) {
          setUserId(users[0].id); // Select first user by default
        }
      } catch (err) {
        console.error('Failed to fetch assignable users', err);
      } finally {
        setLoadingUsers(false);
      }
    };
    
    fetchUsers();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    if (!title.trim() || !userId) {
      setError('Title and Assigned User are required');
      return;
    }

    try {
      setLoading(true);
      await onSubmit({ title, desc, userId });
      setTitle('');
      setDesc('');
      if (assignableUsers.length > 0) {
        setUserId(assignableUsers[0].id);
      }
    } catch (err) {
      setError(err.message || 'Failed to create task');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="bg-white rounded-xl shadow-sm border border-gray-200 p-6 mb-8">
      <div className="mb-6">
        <h2 className="text-xl font-semibold text-gray-900">Create New Task</h2>
        <p className="text-sm text-gray-500">Assign a new task to a user.</p>
      </div>

      {error && (
        <div className="mb-4 p-3 bg-red-50 text-red-700 text-sm rounded-lg border border-red-200">
          {error}
        </div>
      )}

      <form onSubmit={handleSubmit} className="space-y-4">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div>
            <label htmlFor="title" className="block text-sm font-medium text-gray-700 mb-1">Task Title *</label>
            <input
              id="title"
              type="text"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 transition-colors"
              placeholder="e.g., Update documentation"
            />
          </div>
          <div>
            <label htmlFor="userId" className="block text-sm font-medium text-gray-700 mb-1">Assign to (User) *</label>
            {loadingUsers ? (
              <div className="flex items-center px-4 py-2 border border-gray-300 rounded-lg bg-gray-50 text-gray-500">
                <Loader2 className="w-4 h-4 mr-2 animate-spin" /> Loading users...
              </div>
            ) : (
              <select
                id="userId"
                value={userId}
                onChange={(e) => setUserId(e.target.value)}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 transition-colors bg-white appearance-none"
              >
                {assignableUsers.length === 0 && <option value="">No users available</option>}
                {assignableUsers.map(user => (
                  <option key={user.id} value={user.id}>
                    {user.email}
                  </option>
                ))}
              </select>
            )}
          </div>
        </div>
        <div>
          <label htmlFor="desc" className="block text-sm font-medium text-gray-700 mb-1">Description</label>
          <textarea
            id="desc"
            value={desc}
            onChange={(e) => setDesc(e.target.value)}
            rows="3"
            className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 transition-colors resize-none"
            placeholder="Add some details about the task..."
          />
        </div>
        <div className="flex justify-end">
          <button
            type="submit"
            disabled={loading || loadingUsers || assignableUsers.length === 0}
            className="inline-flex items-center gap-2 px-6 py-2.5 bg-indigo-600 hover:bg-indigo-700 text-white font-medium rounded-lg transition-colors disabled:opacity-50"
          >
            {loading ? <Loader2 className="w-5 h-5 animate-spin" /> : <PlusCircle className="w-5 h-5" />}
            Create Task
          </button>
        </div>
      </form>
    </div>
  );
}
