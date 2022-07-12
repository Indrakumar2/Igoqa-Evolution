import React,{ Component } from 'react';
import sanitize from 'sanitize-html';
import QuillComponent from '../../../../common/baseComponents/quill';
import { getlocalizeData } from '../../../../utils/commonUtils';

const localConstant = getlocalizeData();

class EmailTemplates extends Component{
    constructor(props) {
        super(props);
        this.selectedData = '';
        this.selectedEmail  = '';
        this.selectedPlaceholder = '';
        this.state = {
            selectedEmail: ''           
        };
        this.templateValue = '';
        this.options = [];
        // this.props.actions.FetchPlaceholders();
    }
    // componetWillMount(){        
        // const tab = document.querySelectorAll('.tabs');
        // const tabInstances = MaterializeComponent.Tabs.init(tab);
        // const select = document.querySelectorAll('select');
        // const selectInstances = MaterializeComponent.FormSelect.init(select);
        // const datePicker = document.querySelectorAll('.datepicker');
        // const datePickerInstances = MaterializeComponent.Datepicker.init(datePicker);
        // const custModal = document.querySelectorAll('.modal');
        // const custModalInstances = MaterializeComponent.Modal.init(custModal,{ "dismissible":false });
    // }
    componentDidMount (){
        if(this.props.emailTemplateType){
            this.updateEmailTemplateValues(this.props.emailTemplateType);
        }
        else{
            this.updateEmailTemplateValues("");
        }
    }
    updateEmailTemplateValues =(value)=>{
        const { companyEmailTemplates } = this.props;
        this.templateValue = value;        
        if (companyEmailTemplates) {
        
            if (this.templateValue === "CRN") {
                this.selectedEmail = companyEmailTemplates.customerReportingNotificationEmailText;
            }
            if (this.templateValue === "CDR") {
                this.selectedEmail = companyEmailTemplates.customerDirectReportingEmailText;
            }
            if (this.templateValue === "RJVT") {
                this.selectedEmail = companyEmailTemplates.rejectVisitTimesheetEmailText;
            }
            if (this.templateValue === "VCECO") {
                this.selectedEmail = companyEmailTemplates.visitCompletedCoordinatorEmailText;
            }
            if (this.templateValue === "ICAOCO") {
                this.selectedEmail = companyEmailTemplates.interCompanyOperatingCoordinatorEmail;
            }
            if(this.templateValue === ""){
                this.selectedEmail="";
            }
        }
        if(this.templateValue !== ""){
            this.props.actions.FetchPlaceholders(this.templateValue);
        }
        this.props.actions.UpdateEmailTemplate(this.selectedEmail);
        this.props.actions.UpdateEmailTemplateType(this.templateValue);
    }
    changeEmailTemplate = (e) => {
        this.updateEmailTemplateValues(e.target.value);
    }
    render(){
        const { allCompanyEmail } = this.props;
        return(
            <div>
              <div className="customCard mt-0">
                        <h6 className="pl-0 bold"> E-Mail Templates </h6>
               <div className="row">
                    <div className="col s4">
               
                        <label className="customLabel"> {localConstant.companyDetails.emailTemplate.EDIT_EMAIL_TEMPLATE} </label>
                        <select className="browser-default customInputs" onChange={this.changeEmailTemplate} value={this.props.emailTemplateType}>
                        <option value="">Select</option>
                            <optgroup label="Visit">
                                <option value="CRN">Customer Reporting Notification Email</option>
                                <option value="CDR">Customer Direct Reporting Email</option>
                                <option value="RJVT">Rejected Visit / Timesheet Email</option>
                                <option value="VCECO"> Visit Completed Coordinator Email</option>
                            </optgroup>
                            <optgroup label="Assignment">
                                <option value="ICAOCO">Inter Company Operating Coordinator Email</option>
                            </optgroup>
                        </select>
                    </div>
                </div>

                <div className="mb-2">
                <QuillComponent templateValue={this.props.emailTemplateType} text={ sanitize( this.props.emailTemplate ) } readOnly={this.props.pageMode==='View'} />
                </div>

            </div>
        </div>
        );
    }
}

export default EmailTemplates;