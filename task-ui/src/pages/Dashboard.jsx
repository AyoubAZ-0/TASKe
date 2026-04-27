import { useState, useEffect, useCallback } from 'react';
import { useAuth } from '../context/AuthContext';
import { api } from '../api';
import Navbar from '../components/Navbar';
import TaskCard from '../components/TaskCard';
import CreateTaskForm from '../components/CreateTaskForm';
import { Loader2, LayoutDashboard, AlertCircle } from 'lucide-react';

export default function Dashboard() {
  const { user } = useAuth();
  const [tasks, setTasks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchTasks = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      // Assuming getTasks is a generic GET to fetch tasks based on user role
      const data = await api.getTasks(user.id, user.role);
      setTasks(Array.isArray(data) ? data : []);
    } catch (err) {
      setError(err.message || 'Failed to fetch tasks');
      // Set to empty array on error to avoid mapping issues
      setTasks([]);
    } finally {
      setLoading(false);
    }
  }, [user]);

  useEffect(() => {
    fetchTasks();
  }, [fetchTasks]);

  const handleCreateTask = async (taskData) => {
    await api.createTask(taskData.title, taskData.desc, taskData.userId);
    // Refresh tasks after creation
    fetchTasks();
  };

  const handleStatusChange = async (taskId, newStatus) => {
    try {
      await api.updateTaskStatus(taskId, user.id, newStatus);
      // Optimistic update
      setTasks(prev => prev.map(t => t.id === taskId ? { ...t, status: newStatus } : t));
    } catch (err) {
      setError('Failed to update task status');
    }
  };

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      <Navbar />
      
      <main className="flex-1 max-w-7xl w-full mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="mb-8 flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-bold text-gray-900 flex items-center gap-2">
              <LayoutDashboard className="w-8 h-8 text-indigo-600" />
              Dashboard
            </h1>
            <p className="mt-1 text-gray-500">
              {user.role === 'Admin' ? 'Manage system tasks and assignments.' : 'View and update your assigned tasks.'}
            </p>
          </div>
        </div>

        {error && (
          <div className="mb-8 p-4 bg-red-50 border-l-4 border-red-500 rounded-r-lg flex items-start gap-3">
            <AlertCircle className="w-5 h-5 text-red-600 mt-0.5" />
            <p className="text-sm text-red-700">{error}</p>
          </div>
        )}

        {user.role === 'Admin' && (
          <CreateTaskForm onSubmit={handleCreateTask} />
        )}

        <div className="space-y-6">
          <h2 className="text-xl font-semibold text-gray-900 border-b border-gray-200 pb-2">
            {user.role === 'Admin' ? 'All Tasks' : 'My Tasks'}
          </h2>

          {loading ? (
            <div className="flex justify-center items-center py-20">
              <Loader2 className="w-8 h-8 text-indigo-600 animate-spin" />
            </div>
          ) : tasks.length === 0 ? (
            <div className="text-center py-20 bg-white rounded-xl border border-dashed border-gray-300">
              <p className="text-gray-500">No tasks found. {user.role === 'Admin' ? 'Create one above.' : 'You have no assigned tasks.'}</p>
            </div>
          ) : (
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
              {tasks.map(task => (
                <TaskCard 
                  key={task.id} 
                  task={task} 
                  onStatusChange={handleStatusChange} 
                  isUser={user.role === 'User'} 
                />
              ))}
            </div>
          )}
        </div>
      </main>
    </div>
  );
}
