import React, { Fragment } from 'react';
import Draggable from 'react-draggable'; // The default
import { fieldLengthConstants } from '../../../../../../constants/fieldLengthConstants';
const accountRefModal = (props) =>{
    return(
        <Fragment>
        <Draggable handle=".handle">
        <div id="accountReferenceModal" className="modal" ref= {props.accntModal }>
        <form onSubmit={props.formSubmit} id="addAccountReference" className="col s12">
            <div className="modal-content">
                <h6 className="handle m-0">{!props.showButton ?"Add Account Reference":"Edit Account Reference"}  <i class={"zmdi zmdi-close right modal-close"} onClick={ props.clearAccountRef }></i></h6>
                <span class="boldBorder"></span>
                <div className="row mt-3">
                    <div className="col s6">
                        <label htmlFor="companyName" className="customLabel mandate">Company Name</label>
                        <select id="companyName" name="companyCode" className="browser-default" onChange={props.selectChange}>
                            <option value="">Select</option>
                            {props.companyData.map(result => {
                                if (result.companyCode === props.editedAccountRef.companyCode) {
                                    return <option value={result.companyCode} selected>{result.companyName}</option>;
                                }
                                else {
                                    return <option value={result.companyCode}>{result.companyName}</option>;
                                }
                            })}
                        </select>
                    </div>
                    <div className="col s6">
                        <label htmlFor="AccountReference" className="customLabel mandate">Accounts Reference</label>
                        <input autoComplete="off" className="browser-default customInputs validate" maxlength={fieldLengthConstants.Customer.Assignment_Account_Reference.ACCOUNT_REFERENCE_MAXLENGTH} name="accountReferenceValue" onChange={props.inputChange} onInput={props.inputChange} defaultValue={props.editedAccountRef.accountReferenceValue} id="AccountReference" type="text"/>
                    </div>
                </div>
            </div>
            <div className="modal-footer">
                <button type="button" id="cancelAccountReference" onClick={props.clearAccountRef} className="modal-close waves-effect waves-teal btn-small mr-2">Cancel</button>
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
    { props.showAccntOverlay && <div className="customModalOverlay"></div> }
    
    </Fragment>
    );
};

export default accountRefModal;