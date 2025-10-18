import { useQuery } from '@tanstack/react-query'
import './App.css'

interface HealthResponse {
  status: string;
  timestamp: string;
}

const fetchHealth = async (): Promise<HealthResponse> => {
  const response = await fetch('http://localhost:5000/api/health');
  if (!response.ok) {
    throw new Error('Failed to fetch health');
  }
  return response.json();
};

function App() {
  const { data: health, isLoading, isError } = useQuery({
    queryKey: ['health'],
    queryFn: fetchHealth,
  });

  return (
    <div className="App">
      <h1>EzyClassroomz2</h1>
      <div className="card">
        <h2>API Status</h2>
        {isLoading ? (
          <p>Checking API...</p>
        ) : isError ? (
          <p>Failed to connect to API</p>
        ) : health ? (
          <div>
            <p>Status: {health.status}</p>
            <p>Timestamp: {new Date(health.timestamp).toLocaleString()}</p>
          </div>
        ) : null}
      </div>
    </div>
  )
}

export default App
