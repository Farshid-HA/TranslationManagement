import { ChangeEvent, useEffect, useState } from "react";
import { Toast, ToastContainer, Button, Modal, Form, Col, Table } from "react-bootstrap";
import { ResponseApi } from "../models/ResponseApi";
import { JobStatusEnum } from "../enums/JobStatusEnum";
import { TranslationJobModel } from "../models/TranslationJobModel";
import { ReactSearchAutocomplete } from 'react-search-autocomplete'
import { MessageCodeEnum } from "../enums/MessageCodeEnum";

const TranslationJob = () => {

    const [list, setList] = useState<ResponseApi<TranslationJobModel>>({
        recordCount: 0,
        data: [] as TranslationJobModel[],
        page: 1,
        pageCount: 0
    });

    let baseUrl = process.env.REACT_APP_BASE_URL;
    console.log(baseUrl);
    let getUrl = baseUrl + "TranslationJob/GetJobs";
    let addUrl = baseUrl + "TranslationJob/CreateJob";
    let editUrl = baseUrl + "TranslationJob/UpdateJobStatus";
    let getTranslatorUrl = baseUrl + "TranslatorManagement/GetTranslatorsByName";
    const [showInsertModal, setShowInsertModal] = useState(false);
    const [showInsertModalWithFile, setShowInsertModalWithFile] = useState(false);
    const [showUpdateModal, setShowUpdateModal] = useState<boolean>(false);

    const [showToast, setShowToast] = useState(false);
    const [id, setId] = useState<number>(0);
    const [customerName, setCustomerName] = useState<string>("");
    const [status, setStatus] = useState<JobStatusEnum>(0);
    const [originalContent, setOriginalContent] = useState<string>("");
    const [translatedContent, setTranslatedContent] = useState<string>("");
    const [price, setPrice] = useState<Number>(0);
    const [file, setFile] = useState<File>(null);
    const [items, setItems] = useState([]);
    const [translatorId, setTranslatorId] = useState<string>("")



    const [toastTitle, setToastTitle] = useState<string>("");
    const [toastMessage, setToastmessage] = useState<string>("");
    const [search, setSearch] = useState<string>("");
    const [isEdit, setIsEdit] = useState<boolean>(false);

    const handleClose = () => setShowInsertModal(false);
    const autocompleteSelectItem = (item) => setTranslatorId(item.id)

    const handleCloseInserFileModal = () => setShowInsertModalWithFile(false);
    const handleUpdateModalClose = () => setShowUpdateModal(false);
    const handleShow = () => setShowInsertModal(true);

    const [currentPage, setCurrentPage] = useState<number>(1);
    const [pageCount, setPageCount] = useState<number>(0);
    const numberOfPage: number[] = [];
    for (let index = 1; index <= pageCount; index++) {
        numberOfPage.push(index);
    }

    function GetTranslationJob(page: number, search: string = "") {
        fetch(getUrl + "?page=" + page + "&search=" + search).then((res) => {
            return res.json();
        }).then((result) => {

            setList(result.data);
            setCurrentPage(result.data.page);
            setPageCount(result.data.pageCount);

        }).catch((err) => {
            setToastTitle("Error");
            setToastmessage("something wrong please try later");
            console.log(err);
            setShowToast(true);;
        });

    }
    function cleanForm() {
        setCustomerName("");
        setOriginalContent("");
        setTranslatedContent("");
        setStatus(0);
        setPrice(0);
        setId(0);
    }
    function handleSubmit(e) {
        e.preventDefault();
        let translator: TranslationJobModel = {
            id: id,
            customerName: customerName,
            originalContent: originalContent,
            translatedContent: translatedContent,
            status: status,
            price: price
        };
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
            console.log(data);
            setShowInsertModal(false);
            if (data.messageCode == MessageCodeEnum.Success) {
                setToastTitle("Success");
                setToastmessage("Record Insert Successfully.");
                GetTranslationJob(currentPage, "");
            } else if (data.messageCode == MessageCodeEnum.InvalidStatusChange) {
                setToastTitle("Error");
                setToastmessage("Invalid translation job status!.");
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
            setShowToast(true);;
        });

        setShowInsertModal(false);

    }
    function handleSearchByName(event) {
        let name = event.target.value;
        if (name.length > 0) {
            GetTranslationJob(1, name);
            setSearch(name);
        } else {
            GetTranslationJob(currentPage, "");
        }
    }
    function updateStatus(translationJob: TranslationJobModel) {
        setId(Number(translationJob.id));
        setCustomerName(translationJob.customerName);
        setStatus(Number(translationJob.status));
        setIsEdit(true);
        setShowUpdateModal(true);
    }
    function showInsertForm() {
        cleanForm();
        setIsEdit(false);
        setShowInsertModal(true);
    }

    function showInsertFormWithFile() {
        setCustomerName("");
        setFile(null);
        setShowInsertModalWithFile(true);
    }
    function handleSendFile(e) {
        e.preventDefault();
        console.log("handleFile")
        var formData = new FormData();
        formData.append("file", file);
        formData.append("customer", customerName);
        console.log(customerName);
        fetch("CreateJobWithFile", {
            method: 'post',
            body: formData
        }).then((res) => {
            return res.json()
        }).then((data) => {

            setShowInsertModalWithFile(false);
            if (data.messageCode == MessageCodeEnum.Success) {
                setToastTitle("Success");
                setToastmessage("Record Insert Successfully.");
                GetTranslationJob(currentPage, "");
                setShowToast(true);;
            } else if (data.messageCode == MessageCodeEnum.NotFound) {
                setToastTitle("Error");
                setToastmessage("File cant be null");
                setShowToast(true);;
            } else {
                setToastTitle("Error");
                setToastmessage("something wrong please try later");
                setShowToast(true);;
            }
            setShowToast(true);
            cleanForm();
        }).catch((err) => {

            setToastTitle("Error");
            setToastmessage("something wrong please try later");
            console.log(err);
            setShowToast(true);;
        });
        setShowInsertModalWithFile(false);

    }

    function handleFileChange(e: ChangeEvent<HTMLInputElement>) {
        if (e.target.files) {
            setFile(e.target.files[0]);
        }
    }


    function handleOnSearch(string, result: []) {

        fetch(getTranslatorUrl + "?name=" + string).then((res) => {
            return res.json();
        }).then((result) => {

            let itemsArray = [];
            for (let index = 0; index < result.length; index++) {
                var item = {
                    id: result[index].id,
                    name: result[index].name
                };

                itemsArray.push(item);

            }
            setItems(itemsArray);

        }).catch((err) => {
            setToastTitle("Error");
            setToastmessage("something wrong please try later");
            console.log(err);
            setShowToast(true);;
        });

    }



    useEffect(() => {

        fetch(getUrl + "?page=1").then((res) => {
            return res.json();
        }).then((result) => {
            if (result && result.data.data.length > 0) {
                setList(result.data);
                setCurrentPage(result.data.page);
                setPageCount(result.data.pageCount);

            }
        }).catch((err) => {
            setToastTitle("Error");
            setToastmessage("something wrong please try later");
            console.log(err);
            setShowToast(true);;
        });


    }, [])


    return (
        <div className="container">
            <div className="row mt-5 mb-1">
                <Button variant="primary" style={{ width: '180px', marginRight: '5px' }} onClick={showInsertForm}>
                    Create Job
                </Button>

                <Button variant="primary" style={{ width: '180px' }} onClick={showInsertFormWithFile}>

                    Create Job With File
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
                            <th>Customer Name</th>
                            <th>Original Content</th>
                            <th>Status</th>
                            <th>TranslatedContent</th>
                            <th>Price</th>
                        </tr>
                    </thead>
                    {list.data && list.data.length > 0 &&

                        list.data.map((translationJob) => {
                            return (
                                <tbody>
                                    <tr>
                                        <td>{translationJob.customerName}</td>
                                        <td>{translationJob.originalContent}</td>
                                        <td>{JobStatusEnum[translationJob.status]}</td>
                                        <td>{translationJob.translatedContent}</td>
                                        <td>{translationJob.price.toString()}</td>
                                        <td>
                                            <Button variant="primary" onClick={() => updateStatus(translationJob)}>Update Status</Button>
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
                    <Modal.Title>Add Translation</Modal.Title>
                </Modal.Header>

                <Modal.Body>
                    <form onSubmit={handleSubmit}>
                        <table>
                            <tr>
                                <td>
                                    <label className="col-form-label" >Customer Name :</label>
                                </td>
                                <td>
                                    <input type="text" required value={customerName} disabled={isEdit} onChange={(e) => setCustomerName(e.target.value)} className="form-control" id="txtCustomerName" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label className="col-form-label">OriginalContent :</label>
                                </td>
                                <td>
                                    <textarea rows={8} required disabled={isEdit} value={originalContent} onChange={(e) => setOriginalContent(e.target.value)} className="form-control" id="txtOriginalContent" >

                                    </textarea>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label className="col-form-label">Status :</label>
                                </td>
                                <td>
                                    <select className="form-control" disabled={true} onChange={(e) => setStatus(Number(e.target.value))} id="txtStatus">
                                        <option value="0">New</option>
                                        <option value="1">Inprogress</option>
                                        <option value="2">Completed</option>
                                    </select>
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

            <Modal show={showInsertModalWithFile} onHide={handleCloseInserFileModal}>
                <Modal.Header closeButton>
                    <Modal.Title>Add Translation With File</Modal.Title>
                </Modal.Header>

                <Modal.Body>
                    <form onSubmit={handleSendFile}>
                        <table>
                            <tr>
                                <td>
                                    <label className="col-form-label" >Customer Name :</label>
                                </td>
                                <td>
                                    <input type="text" required value={customerName} onChange={(e) => setCustomerName(e.target.value)} className="form-control" id="txtCustomerName" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label className="col-form-label">File :</label>
                                </td>
                                <td>
                                    <input type="file" onChange={handleFileChange} />
                                </td>
                            </tr>

                        </table>
                        <br />
                        <hr />
                        <div style={{ display: 'flex', justifyContent: 'right' }}>
                            <Button variant="secondary" className="mx-1" onClick={handleCloseInserFileModal}>
                                Close
                            </Button>
                            <Button type="submit" variant="primary" >
                                Save Changes
                            </Button>
                        </div>
                    </form>
                </Modal.Body>

            </Modal>

            <Modal show={showUpdateModal} onHide={handleUpdateModalClose}>
                <Modal.Header closeButton>
                    <Modal.Title>Update Translation</Modal.Title>
                </Modal.Header>

                <Modal.Body>
                    <form onSubmit={handleUpdateJob}>
                        <table>
                            <tr>
                                <td>
                                    <label className="col-form-label" >Customer Name :</label>
                                </td>
                                <td>
                                    <input type="text" required value={customerName} disabled={isEdit} onChange={(e) => setCustomerName(e.target.value)} className="form-control" id="txtCustomerName" />
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <label className="col-form-label" >Translator Name :</label>
                                </td>
                                <td>

                                    <ReactSearchAutocomplete onSearch={handleOnSearch} onSelect={autocompleteSelectItem} items={items} autoFocus />

                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <label className="col-form-label">Status :</label>
                                </td>
                                <td>
                                    <select className="form-control" value={status} onChange={(e) => setStatus(Number(e.target.value))} id="txtStatus">
                                        <option value="0">New</option>
                                        <option value="1">Inprogress</option>
                                        <option value="2">Completed</option>
                                    </select>
                                </td>
                            </tr>


                        </table>
                        <br />
                        <hr />
                        <div style={{ display: 'flex', justifyContent: 'right' }}>
                            <Button variant="secondary" className="mx-1" onClick={handleUpdateModalClose}>
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
            GetTranslationJob(currentPage + 1, search);
        } else {
            setCurrentPage(pageCount);
            GetTranslationJob(pageCount, search);
        }

    }
    function previousPage() {
        if (currentPage > 1) {
            setCurrentPage(currentPage - 1)
            GetTranslationJob(currentPage - 1, search);
        } else {
            setCurrentPage(1);
            GetTranslationJob(1, search);
        }

    }
    async function changePage(number: number) {
        console.log(number);
        setCurrentPage(number)
        GetTranslationJob(number, search);
    }


    function handleUpdateJob(e) {
        e.preventDefault();
        var formData = new FormData();
        var obj = {
            jobId: id,
            translatorId: translatorId,
            newStatus: status
        };
        fetch(editUrl, {
            method: 'post',
            headers: {
                Accept: 'application.json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(obj)
        }).then((res) => {
            return res.json();
        }).then((data) => {
            setShowUpdateModal(false);
            if (data.messageCode == MessageCodeEnum.Success) {
                setToastTitle("Success");
                setToastmessage("Record Update Successfully.");
                GetTranslationJob(currentPage, "");
            } else if (data.messageCode == MessageCodeEnum.InvalidStatusChange) {
                setToastTitle("Error");
                setToastmessage("Invalid translation job status!.");
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

        setShowUpdateModal(false);
    }


}

export default TranslationJob
