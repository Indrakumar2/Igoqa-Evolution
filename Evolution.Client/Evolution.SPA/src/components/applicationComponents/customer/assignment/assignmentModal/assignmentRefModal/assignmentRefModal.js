import React,{ Fragment } from 'react';
import Draggable from 'react-draggable'; // The default
const assignmentRefModal = (props) => {
    return (
        <Fragment>
        <Draggable handle=".handle">
        <div id="assignmentReferenceType" className="modal" ref={props.refModal}>
            <form onSubmit={props.formSubmit} id="addAssignmentReferenceType" className="col s12">
                <div className="modal-content">
                    <h6 className="handle m-0">{!props.showButton ?"Add Assignment Reference Type":'Edit Assignment Reference Type'} <i class={"zmdi zmdi-close right modal-close"} onClick={props.clearAssignmentRef}></i></h6>
                    <span class="boldBorder"></span>
                    <div className="row mt-3">
                        <div className="col s6">
                            <label htmlFor="assignmentReferenceType" className="customLabel mandate">Assignment Reference Type</label>
                            <select id="assignmentReferenceType" name="assignmentRefType" className="browser-default" onChange={props.selectChange}>
                                <option value="">Select</option>
                                {props.assignmentRefTypes.map((result) => {
                                    if (result.name === props.editedAssignmentRef.assignmentRefType) {
                                        return <option value={result.name} selected>{result.name}</option>;
                                    }
                                    else {
                                        return <option value={result.name}>{result.name}</option>;
                                    }
                                })}
                            </select>
                        </div>
                    </div>
                </div>
                <div className="modal-footer">
                    <button type="button" id="cancelAssignmentReference" onClick={props.clearAssignmentRef} className="modal-close waves-effect waves-teal btn-small mr-2">Cancel</button>
                    {!props.showButton ?
                        <button type="submit"
                            className="btn-small">
                            Submit
                    </button>
                        :
                        <button type="submit"
                            className="btn-small">
                            Submit
                    </button>
                    }
                </div>
            </form>
        </div>
        </Draggable>
        {props.showOverlay && <div className="customModalOverlay"></div> }
        </Fragment>
    );
};

export default assignmentRefModal;