import { useRef } from 'react';
import './App.css';
import Altcha from './Altcha';

function App() {
    const widgetRef = useRef(null);

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch(`https://localhost:7135/verifySelfHosted`, {
                method: 'POST',
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    "Key": widgetRef.current?.value
                })
            });

            console.log(response);
        } catch (error) {
            console.log(error);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <Altcha ref={widgetRef} />
            <button type="submit">Submit</button>
        </form>
    );
}

export default App;