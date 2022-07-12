import React, { Component, Fragment } from 'react'; 
import CustomTabs from '../../../common/baseComponents/customTab';
import { assignmentLifecycleStatusTabs } from './assignmentLifecycleStatusTabs';
import { SaveBarWithCustomButtons } from '../../applicationComponents/saveBar';
import { getlocalizeData,isEmpty } from '../../../utils/commonUtils';
const localConstant = getlocalizeData();
class AssignmentLifecycleStatus extends Component {
    constructor(props) {
        super(props);
        this.state = {
           
        };
    }
  
  adminSaveHandler = () => {
    this.props.actions.SaveAssignmentLifecycle();
  }
    adminCancelHandler=()=>{

    }
    render() {
       
      const adminSave = [
        {
          name: 'Save',
          clickHandler: () => this.adminSaveHandler(),
          className: "btn-small mr-0 ml-2",         
          isbtnDisable: this.props.isbtnDisable
        }, 
        {
          name: 'Cancel',
          clickHandler: () => this.adminCancelHandler(),
          className: "btn-small mr-0 ml-2 waves-effect modal-trigger",          
          isbtnDisable: this.props.isbtnDisable
        }
     ];      
        return (
        <Fragment>  
          <SaveBarWithCustomButtons
                    currentMenu={localConstant.admin.ADMIN}
                    currentSubMenu={ this.props.currentPage}
                    buttons={adminSave}
                    />
         <div className="row ml-2 mb-0">
           <div className="col s12 pl-0 pr-2 verticalTabs">
           <CustomTabs
               tabsList={assignmentLifecycleStatusTabs}
               moduleName="assignmentLifecycleStatus"           
             />
           </div>
         </div>
       </Fragment>
        );
    }
}

export default AssignmentLifecycleStatus;