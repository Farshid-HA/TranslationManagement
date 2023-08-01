import { Nav } from "react-bootstrap";
import { Link } from "react-router-dom";



const Navbar = () => {
    return (
        <>
       
            <Nav variant="tabs" className="m-5" defaultActiveKey="/home">
                <Nav.Item>
                    <Nav.Link >
                        <Link to="/">Translators </Link>
                    </Nav.Link>
                </Nav.Item>
                <Nav.Item>
                    <Nav.Link >
                        <Link to="/TranslationJob">Translation Job </Link>
                    </Nav.Link>
                </Nav.Item>
              
                   
                
            </Nav>

        </>
    );
};

export default Navbar;