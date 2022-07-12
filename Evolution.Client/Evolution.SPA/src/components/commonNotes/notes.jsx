import React, { Fragment } from 'react';
import CustomInput from '../../common/baseComponents/inputControlls';
import { getlocalizeData } from '../../utils/commonUtils';
import { fieldLengthConstants } from '../../constants/fieldLengthConstants';
const localConstant = getlocalizeData();
export const NoteModalPopup = (props) => {
    return (
        <Fragment>
            <CustomInput
                hasLabel={true}
                label={props.name}
                divClassName='col pl-0 mb-4'
                colSize="col s12"
                type='textarea'
                //required={true}
                name="note"
                id="note"
                rows="5"
                inputClass=" customInputs textAreaInView"
                labelClass="customLabel mandate"
                maxLength={props.isTimesheetNotes ? fieldLengthConstants.common.notes.VISIT_TIMESHEET_MAXLENGTH:fieldLengthConstants.common.notes.NOTES_MAXLENGTH}
                defaultValue={props.viewNotes ? props.viewNotes : ""}
                onValueChange={props.handlerChange}
                onValueBlur={props.handlerChange}
                autofocus="true"
                readOnly={props.readOnly}
            />
        </Fragment>
    );
};
export const NotesAddButton = (props) => {
    return (
        <div className="right-align mt-2" >
            <button href="javascript:void(0);" onClick={props.uploadNoteHandler} disabled={props.interactionMode}
             className="btn-small waves-effect modal-trigger" >{localConstant.commonConstants.ADD}</button>
        </div>
    );
};