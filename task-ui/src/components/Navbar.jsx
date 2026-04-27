import { useAuth } from '../context/AuthContext';
import { LogOut, CheckSquare } from 'lucide-react';
import { useNavigate } from 'react-router-dom';

export default function Navbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <nav className="bg-white border-b border-gray-200 sticky top-0 z-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between h-16">
          <div className="flex items-center">
            <CheckSquare className="h-8 w-8 text-indigo-600" />
            <span className="ml-2 text-xl font-bold text-gray-900">TASKe</span>
          </div>
          {user && (
            <div className="flex items-center gap-4">
              <div className="flex flex-col items-end">
                <span className="text-sm font-medium text-gray-900">{user.email}</span>
                <span className="text-xs text-gray-500 bg-gray-100 px-2 py-0.5 rounded-full uppercase font-semibold">
                  {user.role}
                </span>
              </div>
              <button
                onClick={handleLogout}
                className="p-2 text-gray-400 hover:text-gray-500 hover:bg-gray-100 rounded-full transition-colors"
                title="Logout"
              >
                <LogOut className="h-5 w-5" />
              </button>
            </div>
          )}
        </div>
      </div>
    </nav>
  );
}
