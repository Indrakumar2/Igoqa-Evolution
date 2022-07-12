import { createSelector } from 'reselect';
import { isEmptyOrUndefine, isEmpty } from '../utils/commonUtils';
const fetchCoordinatorDetails = (name, coordinatorData, displayName, fieldName) => {
    for (let i = 0; i < coordinatorData.length; i++) {
        if (coordinatorData[i][displayName] === name) {
            return coordinatorData[i][fieldName];
        }
    }
    return "";
};

const CheckCoordinatorOrOperator = (coordinator,coordinatorsArray,username)=>{
    const  coordinatorUsername = fetchCoordinatorDetails(coordinator,coordinatorsArray, "displayName", "username");
  return (username === coordinatorUsername);
};

function CompanyCheck(company,loggedInCompany){
    return company == loggedInCompany;
}

function interCompanyAssignment(companyCode1, companyCode2) {
    if (companyCode1 !== companyCode2) {
        return true;
    }
    return false;
};

export const isOperator = createSelector(
    CheckCoordinatorOrOperator,
    (isOperator) => {
        return isOperator;
    }
);

export const getCoordinatorData = createSelector(
    fetchCoordinatorDetails,
    (value) => {
        return value;
    }
);

export const isCoordinator = createSelector(
    CheckCoordinatorOrOperator,
    (isCoordinator) => {
        return isCoordinator;
    }
);

export const isOperatorCompany = createSelector(
    CompanyCheck,
    (isOperatorCompany) => {
        return isOperatorCompany;
    }
);

export const isContractHolderCompany = createSelector(
    CompanyCheck,
    (isContractHolder) => {
        return isContractHolder;
    }
);
 
export const isInterCompanyAssignment = createSelector(
    interCompanyAssignment,
    (isInterCompanyAssignment) => {
        return isInterCompanyAssignment;
    }
);

//currentProject Reporting params
export const GetProjectReportingParams = createSelector([
    (state) => state.projectReportParams, (state) => state.businessUnit ],
    (projectReportParams, assignmentProjectBusinessUnit) => {
        if (Array.isArray(projectReportParams) && projectReportParams.length > 0) {
            for (let i = 0, len = projectReportParams.length; i < len; i++) {
                if (projectReportParams[i].name === assignmentProjectBusinessUnit) {
                    return projectReportParams[i];
                }
            }
        }
        return {};
    }
);

//current workflow type params
export const GetWorkflowTypeParams = createSelector([
    (state) => state.workflowTypeParams, (state) => state.workflowType ],
    (workflowTypeParams, assignmentProjectWorkFlow) => {
        if (Array.isArray(workflowTypeParams) && workflowTypeParams.length > 0) {
            for (let i = 0, len = workflowTypeParams.length; i < len; i++) {
                if (workflowTypeParams[i].value === assignmentProjectWorkFlow) {
                    return workflowTypeParams[i];
                }
            }
        }
        return {};
    }
);

export const isARS = createSelector(
    [ GetProjectReportingParams ],
    (projectReportObj) => (
        (projectReportObj && projectReportObj.isARS) ? true : false
    )
);

export const isSettlingTypeMargin = createSelector([ GetProjectReportingParams  ], (projectReportObj) => (
    (projectReportObj && projectReportObj.interCompanyType === "M") ? true : false
)
);

export const isSettlingTypeCost = createSelector([ GetProjectReportingParams ], (projectReportObj) => (
    (projectReportObj && projectReportObj.interCompanyType === "C") ? true : false
)
);

export const isTimesheet = createSelector([ GetWorkflowTypeParams  ], (workflowTypeObj) => (
    (workflowTypeObj && (workflowTypeObj.value === "M" || workflowTypeObj.value === "N")) ? true : false
)
);

export const isVisit = createSelector([ GetWorkflowTypeParams ], (workflowTypeObj) => (
    (workflowTypeObj && (workflowTypeObj.value === "V" || workflowTypeObj.value === "S")) ? true : false
)
);

function AssignmentWithSameCoordinatoor(coordinatorCode1,coordinatorCode2) {
    if(isEmptyOrUndefine(coordinatorCode1) || isEmptyOrUndefine(coordinatorCode2)){
        return false;
    }
    if (coordinatorCode1 === coordinatorCode2) {
        return true;
    }
    return false;
};

export const isAssignmentWithSameCoordinatoor = createSelector(
    AssignmentWithSameCoordinatoor,
    (assignmentWithSameCoordinatoor) => {
      return assignmentWithSameCoordinatoor;
    }
);