import { Clock, CheckCircle, PlayCircle } from 'lucide-react';
import { useState } from 'react';

export default function TaskCard({ task, onStatusChange, isUser }) {
  const [loading, setLoading] = useState(false);

  const handleStatusUpdate = async (newStatus) => {
    setLoading(true);
    await onStatusChange(task.id, newStatus);
    setLoading(false);
  };

  const statusConfig = {
    NotStarted: { color: 'bg-gray-100 text-gray-800', icon: Clock, label: 'Not Started' },
    Ongoing: { color: 'bg-blue-100 text-blue-800', icon: PlayCircle, label: 'Ongoing' },
    Done: { color: 'bg-green-100 text-green-800', icon: CheckCircle, label: 'Done' }
  };

  const currentStatus = statusConfig[task.status] || statusConfig.NotStarted;
  const StatusIcon = currentStatus.icon;

  return (
    <div className="bg-white rounded-xl shadow-sm border border-gray-200 overflow-hidden hover:shadow-md transition-shadow duration-200">
      <div className="p-5">
        <div className="flex justify-between items-start mb-4">
          <h3 className="text-lg font-semibold text-gray-900 line-clamp-1" title={task.title}>
            {task.title}
          </h3>
          <span className={`inline-flex items-center gap-1.5 px-2.5 py-1 rounded-full text-xs font-medium ${currentStatus.color}`}>
            <StatusIcon className="w-3.5 h-3.5" />
            {currentStatus.label}
          </span>
        </div>
        <p className="text-gray-600 text-sm mb-6 line-clamp-3" title={task.description}>
          {task.description || 'No description provided.'}
        </p>
        
        {isUser && task.status !== 'Done' && (
          <div className="mt-4 flex justify-end">
            {task.status === 'NotStarted' && (
              <button
                onClick={() => handleStatusUpdate('Ongoing')}
                disabled={loading}
                className="inline-flex items-center gap-2 px-4 py-2 bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium rounded-lg transition-colors disabled:opacity-50"
              >
                <PlayCircle className="w-4 h-4" />
                Start Task
              </button>
            )}
            {task.status === 'Ongoing' && (
              <button
                onClick={() => handleStatusUpdate('Done')}
                disabled={loading}
                className="inline-flex items-center gap-2 px-4 py-2 bg-green-600 hover:bg-green-700 text-white text-sm font-medium rounded-lg transition-colors disabled:opacity-50"
              >
                <CheckCircle className="w-4 h-4" />
                Mark Done
              </button>
            )}
          </div>
        )}
      </div>
    </div>
  );
}
