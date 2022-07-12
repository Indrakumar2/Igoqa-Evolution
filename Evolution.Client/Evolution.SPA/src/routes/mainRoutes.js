// import React from 'react';
import React from 'react';
import { Route, Switch, Redirect } from 'react-router-dom';
import Login from '../components/viewComponents/login';
import ForgotPassword from '../components/viewComponents/forgotPassword';
import Dashboard from '../components/viewComponents/dashboard';
import Contract from '../views/contractView/contracts';
import ContractDetails from '../components/viewComponents/contracts/contractDetails';
import NoMatch from '../components/applicationComponents/noMatch/noMatch';

//GRM MODULES
import TechnicalSpecialist from '../grm/views/techSpec';
import TechSpecDetails from '../grm/components/techSpec/techSpecDetails';
import QuickSearchView from '../grm/views/quickSearch';
import PreAssignmentView from '../grm/views/preAssignment';
import TimeOffRequestView from '../grm/views/timeOffRequest';
import CompanyDocuments from '../grm/components/techSpec/companyDocuments';
import OpernationalDocuments from '../grm/components/techSpec/operationalDocuments';
import WonLostReport from '../grm/components/techSpec/wonLostReports';
import CompanySpecificMatrixReport from '../grm/components/techSpec/companySpecificMatrixReport';
import TaxonomyReport from '../grm/components/techSpec/taxonomyReport';
import CalendarScheduleDetailsReport from '../grm/components/techSpec/calendarScheduleDetailsReport';

//dasboard Components
import MyTasks from '../components/applicationComponents/dashboard/mytask';
import MySearchView from '../components/applicationComponents/dashboard/mySearch';
import Assignments from '../components/applicationComponents/dashboard/assignments';
import Inactiveassignments from '../components/applicationComponents/dashboard/inactiveassignments';
import VisitStatus from '../components/applicationComponents/dashboard/visitStatus';
import Timesheet from '../components/applicationComponents/dashboard/timesheet';
import ContractsNearExpiry from '../components/applicationComponents/dashboard/contractsNearExpiry';
import DocumentAproval from '../components/applicationComponents/dashboard/documentAproval';
import BudgetMonetary from '../components/applicationComponents/dashboard/budgetMonetary';
import BudgetHours from '../components/applicationComponents/dashboard/budgetHours';
import Documentation from '../components/applicationComponents/dashboard/documentation';

//Company
import Company from '../views/companyView';
import CompanyDetails from '../components/viewComponents/company/companyDetails';

//Customer
import Customer from '../views/customerView';
import CustomerDetails from '../components/viewComponents/customer/customerDetails';

//Project Components
import Project from '../views/projectView';
import ProjectDetail from '../components/viewComponents/projects/projectDetail';

//Supplier Components
import Supplier from '../views/supplierView';
import SupplierDetail from '../components/viewComponents/supplier/supplierDetails';

//Supplier PO Components
import Supplierpo from '../views/supplierpoView';
import SupplierpoDetails from '../components/viewComponents/supplierpo/supplierpoDetails';
//Assignment Components
import {
  AssignmentSearch,
  AssignmentDetail,
} from '../components/viewComponents/assignment';

import {
  AppMainRoutes,
  AppDashBoardRoutes
} from './routesConfig';

import RequireAuthentication from '../components/hoc/requireAuthentication';
import ErrorBoundary from '../components/error/errorBoundary';
import asyncComponent from '../asyncComponent';
import Forbidden from '../components/error/forbidden';

//ADMIN MODULES
import Admin from '../views/adminView';
import AdminUserView from '../views/adminUserView';
import CreateRole from '../components/applicationComponents/admin/createRole';
import CreateUser from '../components/applicationComponents/admin/createUser';

import AssignmentReferenceType from '../components/applicationComponents/admin/manageReferenceType/referenceTypes';
import AssignmentLifecycleAndStatus from '../components/viewComponents/assignmentLifecycleStatusDetails';

//CONSTANT
import { securitymodule, activitycode } from '../constants/securityConstant';

//import CreateUser from '../grm/components/techSpec/techSpecDetails';

//VISIT 
import Visit  from '../views/visitView';
import CreateVisit  from '../components/viewComponents/assignment/assignmentSearch';
import VisitDetails from '../components/viewComponents/visit/visitDetails';

//TIMESHEET
import TimesheetSearch  from '../components/viewComponents/timeSheet/timesheetSearch';
import CreateTimesheet  from '../components/viewComponents/assignment/assignmentSearch';
import TimesheetDetails from '../components/viewComponents/timeSheet/timesheetDetails';
import GlobalCalendar from '../grm/components/techSpec/globalCalendar';
import AuditSearch from '../components/applicationComponents/auditSearch';
import MyCalendar from '../grm/components/techSpec/myCalendar';

//SSRS
import DownloadServerReports from '../components/viewComponents/visitReports/downloadServerReports';

// lazy Load imported components
const Dashboard_Lazy = asyncComponent(() =>
  import('../components/viewComponents/dashboard').then(module => module.default)
);

const Company_Lazy = asyncComponent(() =>
  import('../views/companyView').then(module => module.default)
);

const CompanyDetails_Lazy = asyncComponent(() =>
  import('../components/viewComponents/company/companyDetails').then(module => module.default)
);
const ProjectDetail_Lazy =asyncComponent(() =>
import('../components/viewComponents/projects/projectDetail').then(module => module.default)
);
const Project_Lazy = asyncComponent(() =>
import('../views/projectView').then(module => module.default)
);
//Customer lazy load component
const Customer_Lazy = asyncComponent(() =>
import('../views/customerView').then(module => module.default)
);

const CustomerDetails_Lazy =asyncComponent(() =>
import('../components/viewComponents/customer/customerDetails').then(module => module.default)
);

const AssignmentSearch_Lazy =asyncComponent(() =>
import('../components/viewComponents/assignment').then(module => module.AssignmentSearch)
);

const AssignmentDetail_Lazy =asyncComponent(() =>
import('../components/viewComponents/assignment').then(module => module.AssignmentDetail)
);

const Visit_Lazy = asyncComponent(() => 
  import('../views/visitView').then(module => module.default)
);
const CreateVisit_Lazy = asyncComponent(() => 
  import('../components/viewComponents/assignment').then(module => module.AssignmentSearch)
);
const VisitDetails_Lazy = asyncComponent(() => 
  import('../components/viewComponents/visit/visitDetails').then(module => module.default)
);
const TimesheetSearch_Lazy = asyncComponent(() => 
  import('../components/viewComponents/timeSheet/timesheetSearch').then(module => module.default)
);
const CreateTimesheet_Lazy = asyncComponent(() => 
  import('../components/viewComponents/assignment').then(module => module.AssignmentSearch)
);
const TimesheetDetails_Lazy = asyncComponent(() => 
  import('../components/viewComponents/timeSheet/timesheetDetails').then(module => module.default)
);

export const MainRoutes = () => {
  return (
    <Switch>
      <Route exact path={AppMainRoutes.default} render={() => <Redirect to="/Login" />} /> 
      <Route path={AppMainRoutes.login} component={Login} />
      <Route path={AppMainRoutes.forgotPassword} component={ForgotPassword} />
      <Route path={AppMainRoutes.error} render={() => <ErrorBoundary status={404} />} />
      <Route path={AppMainRoutes.forbidden} component={RequireAuthentication(Forbidden, "Evolution")} />
      <Route path={AppMainRoutes.company} component={RequireAuthentication(Company_Lazy, "Evolution - Company : Search", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.COMPANY)} />
      <Route path={AppMainRoutes.companyDetails} component={RequireAuthentication(CompanyDetails_Lazy, "Evolution - Company : Details", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.COMPANY)} />
      <Route path={AppMainRoutes.customer} component={RequireAuthentication(Customer_Lazy, "Evolution - Customer : Search", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.CUSTOMER)} />
      <Route path={AppMainRoutes.customerDetails} component={RequireAuthentication(CustomerDetails_Lazy, "Evolution - Customer : Details", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.CUSTOMER)} />
      <Route path={AppMainRoutes.dashboard} component={RequireAuthentication(Dashboard_Lazy, "Evolution - Home")} />
      <Route path={AppMainRoutes.contracts} component={RequireAuthentication(Contract, "Evolution - Contract : Search", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.CONTRACT)} />
      <Route path={AppMainRoutes.createContracts} component={RequireAuthentication(ContractDetails, "Evolution - Contract : Add", [ activitycode.NEW ], securitymodule.CONTRACT)} />
      <Route path={AppMainRoutes.editContracts} component={RequireAuthentication(Contract, "Evolution - Contract : Search", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.CONTRACT)} />
      <Route path={AppMainRoutes.viewContracts} component={RequireAuthentication(Contract, "Evolution - Contract : View", [ activitycode.VIEW ], securitymodule.CONTRACT)} />
      <Route path={AppMainRoutes.contractsDetails} component={RequireAuthentication(ContractDetails, "Evolution - Contract : Details", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.CONTRACT)} />
      <Route path={AppMainRoutes.editProject} component={RequireAuthentication(Project_Lazy, "Evolution - Project : Search", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.PROJECT)} />
      <Route path={AppMainRoutes.viewProject} component={RequireAuthentication(Project_Lazy, "Evolution - Project : View", [ activitycode.VIEW ], securitymodule.PROJECT)} />
      <Route path={AppMainRoutes.projectDetail} component={RequireAuthentication(ProjectDetail_Lazy, "Evolution - Project : Details", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.PROJECT)} />
      <Route path={AppMainRoutes.projectSearchContract} component={RequireAuthentication(Contract, "Evolution - Contract : Search", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.PROJECT)} />
      <Route path={AppMainRoutes.searchContract} component={RequireAuthentication(Contract, "Evolution - Contract : Search", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.CONTRACT)} />
      <Route path={AppMainRoutes.createProject} component={RequireAuthentication(ProjectDetail_Lazy, "Evolution - Project : Add", [ activitycode.NEW ], securitymodule.PROJECT)} />
      <Route path={AppMainRoutes.editSupplier} component={RequireAuthentication(Supplier, "Evolution - Supplier : Search", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.SUPPLIER)} />
      <Route path={AppMainRoutes.viewSupplier} component={RequireAuthentication(Supplier, "Evolution - Supplier : Search" ,[ activitycode.VIEW ], securitymodule.SUPPLIER)} />
      <Route path={AppMainRoutes.supplierDetail} component={RequireAuthentication(SupplierDetail, "Evolution - Supplier : Details", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.SUPPLIER)} />
      <Route path={AppMainRoutes.createSupplier} component={RequireAuthentication(SupplierDetail, "Evolution - Supplier : Add", [ activitycode.NEW ], securitymodule.SUPPLIER)} />
      {/**activitycode.LEVEL_0_MODIFY added for D667 */}
      <Route path={AppMainRoutes.techSpec} component={RequireAuthentication(TechnicalSpecialist, "Evolution - Resource",
      [ activitycode.VIEW,activitycode.MODIFY,activitycode.LEVEL_0_MODIFY,activitycode.LEVEL_0_VIEW ,activitycode.LEVEL_1_MODIFY, activitycode.LEVEL_1_VIEW ,activitycode.LEVEL_2_MODIFY ,activitycode.LEVEL_2_VIEW , 
        activitycode.EDIT_SENSITIVE_DOC, activitycode.VIEW_SENSITIVE_DOC, activitycode.EDIT_PAYRATE, activitycode.VIEW_PAYRATE, activitycode.EDIT_TM, activitycode.VIEW_TM ], securitymodule.TECHSPECIALIST)} />
      <Route path={AppMainRoutes.timeOffRequest} component={RequireAuthentication(TimeOffRequestView, "Evolution - Time off request")} />
      <Route path={AppMainRoutes.quickSearch} component={RequireAuthentication(QuickSearchView, "Evolution - Quick Search" ,[ activitycode.NEW ],securitymodule.QUICKSEARCH)} /> {/**D669 */}
      <Route path={AppMainRoutes.preAssignment} component={RequireAuthentication(PreAssignmentView, "Evolution - Pre-Assignment",[ activitycode.NEW ],securitymodule.PREASSIGNMENT)} /> {/**D669 */}
      <Route path={AppMainRoutes.wonLostReport} component={RequireAuthentication(WonLostReport,"Evolution - Won Lost Report")}/>
      <Route path={AppMainRoutes.calendarScheduleReport} component={RequireAuthentication(CalendarScheduleDetailsReport,"Evolution - Calendar Schedule Report")}/>
      <Route path={AppMainRoutes.createProfile} component={RequireAuthentication(TechSpecDetails, "Evolution - CreateProfile", [ activitycode.NEW ], securitymodule.TECHSPECIALIST)} />
      <Route path={AppMainRoutes.companySpecificMatrixReport} component={RequireAuthentication(CompanySpecificMatrixReport,"Evolution - Company Specific Matrix Report")}/>
      <Route path={AppMainRoutes.taaxonomyReport} component={RequireAuthentication(TaxonomyReport,"Evolution - Taxonomy Report")}/>
     {/**activitycode.LEVEL_0_MODIFY added for D667 */}
      <Route path={AppMainRoutes.profileDetails} component={RequireAuthentication(TechSpecDetails, "Evolution - ProfileDetails",[ activitycode.VIEW,activitycode.MODIFY,activitycode.LEVEL_0_MODIFY,activitycode.LEVEL_0_VIEW ,activitycode.LEVEL_1_MODIFY, activitycode.LEVEL_1_VIEW ,activitycode.LEVEL_2_MODIFY ,activitycode.LEVEL_2_VIEW , 
        activitycode.EDIT_SENSITIVE_DOC, activitycode.VIEW_SENSITIVE_DOC, activitycode.EDIT_PAYRATE, activitycode.VIEW_PAYRATE, activitycode.EDIT_TM, activitycode.VIEW_TM ], [ securitymodule.MYPROFILE ,securitymodule.TECHSPECIALIST ])} />
      <Route path={AppMainRoutes.searchAssignment} component={RequireAuthentication(AssignmentSearch_Lazy, "Evolution - Assignment : Search", [ activitycode.VIEW,activitycode.MODIFY,activitycode.VIEW_ALL ], securitymodule.ASSIGNMENT)} />
      <Route path={AppMainRoutes.visitSearchAssignment} component={RequireAuthentication(AssignmentSearch_Lazy, "Evolution - Assignment : Search", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.VISIT)} />
      <Route path={AppMainRoutes.timesheetSearchAssignment} component={RequireAuthentication(AssignmentSearch_Lazy, "Evolution - Assignment : Search", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.TIMESHEET)} />
      <Route path={AppMainRoutes.assignmentSearchProject} component={RequireAuthentication(Project_Lazy, "Evolution - Project : Search", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.ASSIGNMENT)} />
      <Route path={AppMainRoutes.createSupplierPO} component={RequireAuthentication(Project_Lazy, "Evolution - Project : Search", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.SUPPLIERPO)} />
      <Route path={AppMainRoutes.searchProject} component={RequireAuthentication(Project_Lazy, "Evolution - Project : Search", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.PROJECT)} />
      <Route path={AppMainRoutes.createAssignment} component={RequireAuthentication(AssignmentDetail_Lazy, "Evolution - Assignment : Add", [ activitycode.NEW ], securitymodule.ASSIGNMENT)} />
      <Route path={AppMainRoutes.editAssignment} component={RequireAuthentication(AssignmentDetail_Lazy, "Evolution - Edit Assignment", [ activitycode.VIEW,activitycode.MODIFY,activitycode.VIEW_ALL ], securitymodule.ASSIGNMENT)} />
      <Route path={AppMainRoutes.editSupplierPO} component={RequireAuthentication(Supplierpo, "Evolution - Supplier PO : Search", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.SUPPLIERPO)} />
      <Route path={AppMainRoutes.supplierPoDetails} component={RequireAuthentication(SupplierpoDetails, "Evolution - Supplier PO : Details", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.SUPPLIERPO)} />
      <Route path={AppMainRoutes.auditSearch} component={RequireAuthentication(AuditSearch, "Evolution - Admin : Audit Search")} />
      <Route path={AppMainRoutes.userRoles} component={RequireAuthentication(Admin, "Evolution - Admin : Manage Security",[ activitycode.VIEW,activitycode.MODIFY ], securitymodule.SECURITY)} />
      <Route path={AppMainRoutes.users} component={RequireAuthentication(AdminUserView, "Evolution - Admin : Manage Security",[ activitycode.VIEW,activitycode.MODIFY ], securitymodule.SECURITY)} />
      <Route path={AppMainRoutes.createRole} component={RequireAuthentication(CreateRole, "Evolution - Admin : Manage Security",[ activitycode.NEW ],securitymodule.SECURITY)} />
      <Route path={AppMainRoutes.updateRole} component={RequireAuthentication(CreateRole, "Evolution - Admin : Manage Security",[ activitycode.VIEW,activitycode.MODIFY ], securitymodule.SECURITY)} />
      <Route path={AppMainRoutes.createUser} component={RequireAuthentication(CreateUser, "Evolution - Admin : Manage Security",[ activitycode.NEW ],securitymodule.SECURITY)} />
      <Route path={AppMainRoutes.updateUser} component={RequireAuthentication(CreateUser, "Evolution - Admin : Manage Security",[ activitycode.VIEW,activitycode.MODIFY ], securitymodule.SECURITY)} />
      <Route path={AppMainRoutes.manageReferencesType} component={RequireAuthentication(AssignmentReferenceType, "Evolution - Admin : Manage References Type")} />
      <Route path={AppMainRoutes.assignmentLifecycleAndStatus} component={RequireAuthentication(AssignmentLifecycleAndStatus, "Evolution - Admin : Manage Assignment Lifecycle and Status")} />
      <Route path={AppMainRoutes.intertekCompanyDocuments} component={RequireAuthentication(CompanyDocuments, "Evolution - TechSpec")} />
      <Route path={AppMainRoutes.intertekOperationalDocuments} component={RequireAuthentication(OpernationalDocuments, "Evolution - TechSpec")} />
      <Route path={AppMainRoutes.visitSearch} component={RequireAuthentication(Visit_Lazy,"Evolution - Visit Search", [ activitycode.VIEW,activitycode.MODIFY ], securitymodule.VISIT)} />    
      <Route path={AppMainRoutes.createVisit} component={RequireAuthentication(CreateVisit_Lazy,"Evolution - Create Visit",[ activitycode.NEW ],securitymodule.VISIT)} />    
      <Route path={AppMainRoutes.visitDetails} component={RequireAuthentication(VisitDetails_Lazy,"Evolution - Visit Details",[ activitycode.MODIFY,activitycode.VIEW ],securitymodule.VISIT)} />    
      <Route path={AppMainRoutes.createTimesheet} component={RequireAuthentication(CreateTimesheet_Lazy,"Evolution - Create Timesheet",[ activitycode.NEW ],securitymodule.TIMESHEET)} />    
      <Route path={AppMainRoutes.timesheetSearch} component={RequireAuthentication(TimesheetSearch_Lazy,"Evolution - Timesheet Search",[ activitycode.MODIFY,activitycode.VIEW ],securitymodule.TIMESHEET)} />    
      <Route path={AppMainRoutes.timesheetDetails} component={RequireAuthentication(TimesheetDetails_Lazy,"Evolution - Timesheet Details",[ activitycode.MODIFY,activitycode.VIEW ],securitymodule.TIMESHEET)} />  
      <Route path={AppMainRoutes.downloadServerReport} component={RequireAuthentication(DownloadServerReports,"Evolution - Download Server Report")} />
      {/* Calendar - CR - Request #1 */}
      <Route path={AppMainRoutes.companyCalendar} component={RequireAuthentication(GlobalCalendar,"Evolution - Company Calendar")} />    
      <Route path={AppMainRoutes.myCalendar} component={RequireAuthentication(MyCalendar,"Evolution - My Calendar")} />    
      <Route component={NoMatch} />
    </Switch>
  );
};
export const DashboardRoutes = () => {
  return (
    <Switch>
      <Route exact path={'/Dashboard'} component={Assignments} />
      <Route exact path={AppDashBoardRoutes.mytasks} component={MyTasks} />
      <Route exact path={AppDashBoardRoutes.mysearch} component={MySearchView} />
      <Route exact path={AppDashBoardRoutes.assignments} component={Assignments} />
      <Route exact path={AppDashBoardRoutes.inactiveassignments} component={Inactiveassignments} />
      <Route exact path={AppDashBoardRoutes.visitStatus} component={VisitStatus} />
      <Route exact path={AppDashBoardRoutes.timesheet} component={Timesheet} />
      <Route exact path={AppDashBoardRoutes.contractsNearExpiry} component={ContractsNearExpiry} />
      <Route exact path={AppDashBoardRoutes.documentAproval} component={DocumentAproval} />
      <Route exact path={AppDashBoardRoutes.budgetMonetary} component={BudgetMonetary} />
      <Route exact path={AppDashBoardRoutes.BudgetHours} component={BudgetHours} />
      <Route exact path={AppDashBoardRoutes.documentation} component={Documentation} />
      {/* <Redirect exact from={'/'+AppMainRoutes.dashboard} to={AppDashBoardRoutes.assignments} /> */}
      <Route component={NoMatch} />
    </Switch>
  );
};
