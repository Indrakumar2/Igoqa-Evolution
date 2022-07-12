import React, { Component, Fragment } from 'react';
import { HeaderData } from './headerData.js';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import Modal from '../../../../common/baseComponents/modal';
import dateUtil from '../../../../utils/dateUtil';
import { getlocalizeData, isEmpty ,bindAction } from '../../../../utils/commonUtils';
import CustomModal from '../../../../common/baseComponents/customModal';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import {
    NoteModalPopup,
    NotesAddButton
} from '../../../commonNotes/notes';
import PropTypes from 'prop-types';
import moment from 'moment';

const localConstant = getlocalizeData();
class Notes extends Component {
    constructor(props) {
        super(props);
        this.updatedData = {};
        this.state = {
            isShowNoteModal:false,
            isNoteModelAddView:false
        };
        this.noteAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.hideModal,
                btnID: "cancelNoteSubmit",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.notesSubmitHandler,
                btnID: "noteAddSubmit",
                btnClass: "waves-effect waves-teal btn-small mr-2",
                showbtn: true
            }
        ];
        this.NoteViewButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.hideModal,
                btnID: "cancelNoteSubmit",
                btnClass: "modal-close waves-effect waves-teal  btn-small  mr-2",
                showbtn: true,
                type:"button"
            }
        ];
    }

    notesSubmitHandler = (e) => {
        if (!isEmpty(this.updatedData.note)) {
            const editedData=Object.assign({},this.viewRowData,this.updatedData);
            e.preventDefault();
            const date = moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT);
            if(editedData.assignmnetNoteId || editedData.assignmnetNoteUniqueId){ //D661 issue8
                if(editedData.recordStatus !== "N")
                    editedData.recordStatus="M";
                editedData.createdOn = date;
                editedData.modifiedBy = this.props.loggedInUser;
                this.props.actions.EditAssignmentNotesDetails(editedData);
                this.hideModal(e);                
            }else {
                this.updatedData["recordStatus"] = "N";
                this.updatedData["createdBy"] = this.props.loggedInUser;
                this.updatedData["userDisplayName"] = this.props.loggedInUserName;
                this.updatedData["createdOn"] = date;
                this.updatedData["note"] = this.updatedData.note;
                this.updatedData["assignmentId"] = this.props.assignmentId;
                this.updatedData["assignmnetNoteUniqueId"] = Math.floor(Math.random() * (Math.pow(10, 5)));  //D661 issue8
                this.props.actions.AddAssignmentNotesDetails(this.updatedData);
                this.updatedData = {};
                this.hideModal(e);
            }  
        }
        else
        {
            IntertekToaster(localConstant.validationMessage.ASSIGNMENT_DETAILS_ASSIGNMENT_NOTES,"warningToast NotesVal");            
            e.preventDefault();
        }        
        
    }
    handlerChange = (e) => {
        this.updatedData[e.target.name] = e.target.value;
    }
    
    inputChangeHandler = (e) => {
        const fieldValue = e.target[e.target.type === "checkbox" ? "checked" : "value"];
        const fieldName = e.target.name;
        const result = { value: fieldValue, name: fieldName };
        return result;
    }
    /* Form Input data Change Handler*/
    formInputChangeHandler = (e) => {
        const result = this.inputChangeHandler(e);
        this.updatedData[result.name] = result.value;
    }
    uploadNoteHandler = () => {
        this.setState({
            isShowNoteModal:true,
            isNoteModelAddView:true
        });
    }
    noteHangleChange = (e) => {
        const result = this.inputChangeHandler(e);
        this.updatedData[e.target.name] = result;
        this.updatedData["recordStatus"] = "N";
        this.props.actions.AddAssignmentNotesDetails(this.updatedData);
        this.updatedData = {};
    }
    hideModal = (e) => {
        e.preventDefault();
        this.setState({
            isShowNoteModal:false,
            isNoteModelAddView:false
        });
        this.updatedData = {};
        this.viewRowData={};
    }  
    viewRowHandler = (data) => {
        this.setState({
            isShowNoteModal: true,
            isNoteModelAddView:false,
            isbtnDisable: false
        }); //D661 issue8
        //Commented because currently on hold
        // if(data.createdBy == this.props.loggedInUser){
        //     this.setState({
        //         isNoteModelAddView:true,
        //     });
        // }
        this.viewRowData = data;
    }
    render() {
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        const { assignmentsNotesData } = this.props;
        const AssignmentNotesHeaderData=HeaderData;
        bindAction(AssignmentNotesHeaderData, "ViewAssignmentNotes", this.viewRowHandler);
        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                <div className="customCard">
                    <h6 className="bold col s3 pl-0 pr-3">{localConstant.notes.ASSIGNMENT_NOTES}</h6>
                    <CardPanel className="white lighten-4 black-text" colSize="s12">
                        <ReactGrid gridRowData={assignmentsNotesData}
                            gridColData={AssignmentNotesHeaderData}
                            handlerChange={this.noteHangleChange}
                            onRef={ref => { this.child = ref; }}
                            paginationPrefixId={localConstant.paginationPrefixIds.assignmentNotes} />
                        {this.props.pageMode !== localConstant.commonConstants.VIEW &&
                            <NotesAddButton uploadNoteHandler={this.uploadNoteHandler}
                                interactionMode={this.props.interactionMode}
                            />}
                    </CardPanel>
                    {this.state.isShowNoteModal &&
                    <Modal id="contractDocModalPopup"
                        title={'Notes'}
                        modalClass="projectNoteModal"
                        buttons={this.state.isNoteModelAddView?this.noteAddButtons:this.NoteViewButtons}
                        isShowModal={this.state.isShowNoteModal}>
                        <NoteModalPopup
                          name={localConstant.notes.ASSIGNMENT_NOTES}
                          handlerChange={(e) => this.formInputChangeHandler(e)}
                          viewNotes={this.viewRowData && this.viewRowData.note}
                          readOnly={this.state.isNoteModelAddView?false:true}
                        />
                    </Modal>}
                </div>
            </Fragment>

        );
    }
}

export default Notes;
Notes.propTypes = {
    assignmentsNotesData: PropTypes.array
};

Notes.defaultProps = {
    assignmentsNotesData: []
};