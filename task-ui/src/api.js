const API_BASE = 'http://localhost:5000/api';

const handleResponse = async (res) => {
  if (!res.ok) {
    const error = await res.json().catch(() => ({}));
    throw new Error(error.message || 'An error occurred');
  }
  const text = await res.text();
  return text ? JSON.parse(text) : {};
};

export const api = {
  login: async (email, password) => {
    const res = await fetch(`${API_BASE}/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password }),
    });
    return handleResponse(res);
  },

  register: async (email, password, role) => {
    const res = await fetch(`${API_BASE}/auth/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password, role }),
    });
    return handleResponse(res);
  },

  createTask: async (title, desc, userId) => {
    const res = await fetch(`${API_BASE}/tasks`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ title, desc, userId }),
    });
    return handleResponse(res);
  },

  updateTaskStatus: async (taskId, userId, status) => {
    const res = await fetch(`${API_BASE}/tasks/${taskId}/status?userId=${userId}&status=${status}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' }
    });
    return handleResponse(res);
  },

  getTasks: async (userId, role) => {
    const res = await fetch(`${API_BASE}/tasks?userId=${userId}&role=${role}`, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' }
    });
    return handleResponse(res);
  },

  getAssignableUsers: async () => {
    const res = await fetch(`${API_BASE}/users/assignable`, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' }
    });
    return handleResponse(res);
  }
};
