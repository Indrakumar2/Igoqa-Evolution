import reduceReducers from 'reduce-reducers';
import { UserReducer } from '../security/userReducer';
import { getInitialUserDetailJson } from '../../utils/jsonUtil';

const userInitialState = {
    userLandingPageData: [],
    companyOffices: [],
    roles: [],
    userDetailData: getInitialUserDetailJson(),
    isUserDataChanged:true
};

export default reduceReducers(
    UserReducer,
    userInitialState,
);