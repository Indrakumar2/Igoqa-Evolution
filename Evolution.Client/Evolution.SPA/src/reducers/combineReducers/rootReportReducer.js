import reduceReducers from 'reduce-reducers';
import { visitReports } from '../reports/visitReportReducer';

const initialState = {
customerList:[],
customerContracts:[],
contractProjects:[],
coordinatorCustomerList:[],
serverReportData:[],
coordinators:[]
};

export default reduceReducers(
    visitReports,
    initialState,    
);