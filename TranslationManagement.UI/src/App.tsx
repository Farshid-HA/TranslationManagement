import { Route, Routes } from 'react-router-dom';
import './App.css';
import TranslationJob from './components/TranslationJob.component';
import TranslatorManagement from './components/TranslatorManagement.component';
import Navbar from './components/Navbar.component';

const App = () => {
  return (
    <>
      <Navbar />
      <Routes>
            <Route path="/" element={<TranslatorManagement />} />
            <Route path="/TranslationJob" element={<TranslationJob />} />
            
         </Routes>

    </>

  );
}

export default App;
