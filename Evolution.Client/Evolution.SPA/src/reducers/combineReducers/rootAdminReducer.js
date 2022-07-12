import reduceReducers from 'reduce-reducers';
import { ViewRoleReducer } from '../viewRole/viewRoleReducer';
import { LifeCycleReducer } from '../admin/assignmnetLifecycleReducer';
import { AssignmentStatusReducer } from '../admin/assignmentStatusReducer';
import { ReferenceTypeReducer } from './../admin/referenceTypeReducer';
//import { CreateRoleReducer } from '../createRole/createRoleReducer';

const initialState = {
    userLandingPageData:[],
    viewRole:[],
    moduleList: [],
    moduleListData: [],
    moduleActivityData: [],
    roleActivityData: [],
    selectedRole:{}
};

export default reduceReducers(
    ViewRoleReducer,
    LifeCycleReducer,
    AssignmentStatusReducer,
    ReferenceTypeReducer,
    initialState,    
);