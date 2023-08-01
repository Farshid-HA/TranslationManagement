import { useEffect, useState } from "react";
import { TranslatorModel } from "../models/TranslatorModel";
import { Toast, ToastContainer, Button, Modal, Form, Col, Table } from "react-bootstrap";
import { ResponseApi } from "../models/ResponseApi";
import { TranslatorStatusEnum } from "../enums/TranslatorStatusEnum";
import { MessageCodeEnum } from "../enums/MessageCodeEnum";




const TranslatorManagement = () => {

    const [list, setList] = useState<ResponseApi<TranslatorModel>>({
        recordCount: 0,
        data: [] as TranslatorModel[],
        page: 1,
        pageCount: 0
    });

    let baseUrl = process.env.REACT_APP_BASE_URL;
    let getUrl = baseUrl + "TranslatorManagement/GetTranslators";
    let addUrl = baseUrl + "TranslatorManagement/AddTranslator";
    let editUrl = baseUrl + "TranslatorManagement/UpdateTranslatorStatus";
    const [showInsertModal, setShowInsertModal] = useState(false);
    const [showToast, setShowToast] = useState(false);
    const [id, setId] = useState<number>(0);
    const [Name, setName] = useState<string>("");
    const [HourlyRate, setHourlyRate] = useState<string>("");
    const [Status, setStatus] = useState<TranslatorStatusEnum>(0);
    const [CreadiCardtNumber, setCreadiCardtNumber] = useState<string>("");
    const [toastTitle, setToastTitle] = useState<string>("");
    const [toastMessage, setToastmessage] = useState<string>("");
    const [search, setSearch] = useState<string>("");
    const [isEdit, setIsEdit] = useState<boolean>(false);

    const handleClose = () => setShowInsertModal(false);
    const handleShow = () => setShowInsertModal(true);

    const [currentPage, setCurrentPage] = useState<number>(1);
    const [pageCount, setPageCount] = useState<number>(0);
    const numberOfPage: number[] = [];
    for (let index = 1; index <= pageCount; index++) {
        numberOfPage.push(index);
    }

    function GetTranslators(page: number, search: string = "") {
        fetch(getUrl + "?page=" + page + "&search=" + search).then((res) => {
            return res.json();
        }).then((result) => {
            console.log(result.data);
            setList(result.data);
            setCurrentPage(result.data.page);
            setPageCount(result.data.pageCount);

        }).catch((err) => {
            setToastTitle("Error");
            setToastmessage("something wrong please try later");
            console.log(err);
            setShowToast(true);
        });

    }
    function cleanForm() {
        setName("");
        setHourlyRate("");
        setCreadiCardtNumber("");
        setStatus(0);
    }
    function handleSubmit(e) {
        e.preventDefault();
        let translator: TranslatorModel = {
            id: id,
            name: Name,
            creditCardNumber: CreadiCardtNumber,
            hourlyRate: HourlyRate,
            status: Status
        };
        console.log(translator);

        if (!isEdit) {
            fetch(addUrl, {
                method: 'POST',
                headers: {
                    Accept: 'application.json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(translator)
            }).then((res) => {
                return res.json()
            }).then((data) => {
                setShowInsertModal(false);
                if (data.messageCode == MessageCodeEnum.Success) {
                    setToastTitle("Success");
                    setToastmessage("Record Insert Successfully.");
                    GetTranslators(currentPage, "");
                } else {
                    setToastTitle("Error");
                    setToastmessage("something wrong please try later");

                }
                setShowToast(true);
                cleanForm();
            }).catch((err) => {
                setToastTitle("Error");
                setToastmessage("something wrong please try later");
                console.log(err);
                setShowToast(true);
            });

            setShowInsertModal(false);
        } else {
            fetch(editUrl + "?translator=" + translator.id + "&newStatus=" + translator.status, {
                method: 'POST',
                headers: {
                    Accept: 'application.json',
                    'Content-Type': 'application/json'
                },

            }).then((res) => {
                return res.json()
            }).then((data) => {
                if (data.messageCode == MessageCodeEnum.Success) {
                    setToastTitle("Success");
                    setToastmessage("Record Update Successfully.");
                    GetTranslators(currentPage, "");
                } else {
                    setToastTitle("Error");
                    setToastmessage("something wrong please try later");
                }
                setShowToast(true);

            }).catch((err) => {
                setToastTitle("Error");
                setToastmessage("something wrong please try later");
                console.log(err);
                setShowToast(true);
            });

            cleanForm();
            setIsEdit(false);
            setShowInsertModal(false);
        }
    }
    function handleSearchByName(event) {
        let name = event.target.value;
        if (name.length > 0) {
            GetTranslators(1, name);
            setSearch(name);
        } else {
            GetTranslators(currentPage, "");
        }
    }
    function updateStatus(translator: TranslatorModel) {
        setId(Number(translator.id));
        setName(translator.name);
        setCreadiCardtNumber(translator.creditCardNumber);
        setStatus(translator.status);
        setHourlyRate(translator.hourlyRate);
        setIsEdit(true);
        setShowInsertModal(true);
    }
    function showInsertForm() {
        cleanForm();
        setIsEdit(false);
        setShowInsertModal(true);
    }


    useEffect(() => {

        fetch(getUrl + "?page=1").then((res) => {
            return res.json();
        }).then((result) => {
            console.log(result.data);
            if (result && result.data.data.length > 0) {
                setList(result.data);
                setCurrentPage(result.data.page);
                setPageCount(result.data.pageCount);
            }
        }).catch((err) => {
            setToastTitle("Error");
            setToastmessage("something wrong please try later");
            console.log(err);
            setShowToast(true);
        });


    }, [])


    return (
        <div className="container">
            <div className="row mt-5 mb-2 col-2">
                <Button variant="primary" onClick={showInsertForm}>
                    Add
                </Button>
            </div>
            <div className="row my-3">

                <Col sm={5} m-0>
                    <Form.Control
                        type="text"
                        id="txtSearch"
                        aria-describedby="passwordHelpBlock"
                        onChange={handleSearchByName}
                        className="col-4"
                        placeholder="Search..."
                    />
                </Col>
            </div>
            <div className="row">

                <Table bordered hover>
                    <thead >
                        <tr>
                            <th>Name</th>
                            <th>HourlyRate</th>
                            <th>Status</th>
                            <th>CreditCardNumber</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    {list.data && list.data.length > 0 &&

                        list.data.map((translator) => {
                            return (
                                <tbody>
                                    <tr>
                                        <td>{translator.name}</td>
                                        <td>{translator.hourlyRate}</td>
                                        <td>{TranslatorStatusEnum[translator.status]}</td>
                                        <td>{translator.creditCardNumber}</td>
                                        <td>
                                            <Button variant="primary" onClick={() => updateStatus(translator)}>Update Status</Button>
                                        </td>
                                    </tr>

                                </tbody>
                            )
                        })
                    }

                </Table>
            </div>

            <div className="row m-5">
                {list && list.pageCount > 1 &&
                    <div className="md-m-5 sm-m-1 ">
                        <nav aria-label="..." className="Page navigation example">
                            <ul className="pagination flex-wrap">
                                <li className="page-item">
                                    <a className="page-link" href="#" onClick={previousPage}>Previous</a>
                                </li>
                                {
                                    numberOfPage?.map((number) => {
                                        return (
                                            <li className={`page-item ${currentPage === number ? 'active' : ''}`} onClick={() => changePage(number)}><a className="page-link" href="#">{number}</a></li>
                                        )
                                    })
                                }
                                <li className="page-item">
                                    <a className="page-link" href="#" onClick={nextPage}>Next</a>
                                </li>
                            </ul>
                        </nav>
                    </div>
                }
            </div>



            <Modal show={showInsertModal} onHide={handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>Add Translator</Modal.Title>
                </Modal.Header>

                <Modal.Body>
                    <form onSubmit={handleSubmit}>
                        <table>
                            <tr>
                                <td>
                                    <label className="col-form-label" >Name :</label>
                                </td>
                                <td>
                                    <input type="text" required value={Name} disabled={isEdit} onChange={(e) => setName(e.target.value)} className="form-control" id="txtName" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label className="col-form-label">HourlyRate :</label>
                                </td>
                                <td>
                                    <input type="number" required disabled={isEdit} value={HourlyRate} onChange={(e) => setHourlyRate(e.target.value)} className="form-control" id="txtHourlyRate" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label className="col-form-label">Status :</label>
                                </td>
                                <td>
                                    <select className="form-control" value={Status} onChange={(e) => setStatus(Number(e.target.value))} id="txtStatus">
                                        <option value="0">Applicant</option>
                                        <option value="1">Certified</option>
                                        <option value="2">Deleted</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label className=" col-form-label">CreditCardNumber :</label>
                                </td>
                                <td>
                                    <input type="number" required disabled={isEdit} className="form-control" value={CreadiCardtNumber} onChange={(e) => setCreadiCardtNumber(e.target.value)} id="txtCreditCardNumber" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <hr />
                        <div style={{ display: 'flex', justifyContent: 'right' }}>
                            <Button variant="secondary" className="mx-1" onClick={handleClose}>
                                Close
                            </Button>
                            <Button type="submit" variant="primary" >
                                Save Changes
                            </Button>
                        </div>
                    </form>
                </Modal.Body>

            </Modal>


            <ToastContainer position="top-end" className="p-3" style={{ zIndex: 1 }}>
                <Toast show={showToast} onClose={() => setShowToast(false)} delay={3000} autohide>
                    <Toast.Header>
                        <img
                            src="holder.js/20x20?text=%20"
                            className="rounded me-2"
                            alt=""
                        />
                        <strong className="me-auto">{toastTitle}</strong>

                    </Toast.Header>
                    <Toast.Body>{toastMessage}</Toast.Body>
                </Toast>
            </ToastContainer>

        </div >

    )
    function nextPage() {
        if (currentPage < pageCount!) {
            setCurrentPage(currentPage + 1)
            GetTranslators(currentPage + 1, search);
        } else {
            setCurrentPage(pageCount);
            GetTranslators(pageCount, search);
        }

    }
    function previousPage() {
        if (currentPage > 1) {
            setCurrentPage(currentPage - 1)
            GetTranslators(currentPage - 1, search);
        } else {
            setCurrentPage(1);
            GetTranslators(1, search);
        }

    }
    async function changePage(number: number) {
        console.log(number);
        setCurrentPage(number)
        GetTranslators(number, search);
    }

}

export default TranslatorManagement
