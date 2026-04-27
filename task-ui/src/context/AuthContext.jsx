import { createContext, useContext, useState, useEffect } from 'react';
import { api } from '../api';

const AuthContext = createContext();

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Check local storage for persisted user
    const storedUser = localStorage.getItem('taskUser');
    if (storedUser) {
      setUser(JSON.parse(storedUser));
    }
    setLoading(false);
  }, []);

  const login = async (email, password) => {
    try {
      const data = await api.login(email, password);
      // Backend returns: { id, email, role }
      setUser(data);
      localStorage.setItem('taskUser', JSON.stringify(data));
      return data;
    } catch (error) {
      throw error;
    }
  };

  const register = async (email, password, role) => {
    try {
      const data = await api.register(email, password, role);
      // Wait for user to login separately or login immediately.
      // Requirements say "Redirect to dashboard after login", usually register doesn't auto-login unless specified, 
      // but let's just return the data.
      return data;
    } catch (error) {
      throw error;
    }
  };

  const logout = () => {
    setUser(null);
    localStorage.removeItem('taskUser');
  };

  const value = {
    user,
    login,
    register,
    logout,
    loading
  };

  return (
    <AuthContext.Provider value={value}>
      {!loading && children}
    </AuthContext.Provider>
  );
};
