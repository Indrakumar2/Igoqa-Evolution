import React, { Component, Fragment } from 'react';
import MaterializeComponent from 'materialize-css';
import IntertekToaster from '../../common/baseComponents/intertekToaster';

class CompanyList extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isDashboard: true
        };
    }
    // componentWillMount() {
    //     this.props.actions.FetchUserRoleCompanyList(this.props.userName).then(x => {
    //         this.props.actions.FetchCompanyList();
    //     });
    // }
    componentDidMount() {
        this.props.actions.FetchUserRoleCompanyList(this.props.userName).then(x => {
            this.props.actions.FetchCompanyList();
        });
        if ((this.props.history.location.pathname).includes("Dashboard")) {
            this.setState({ isDashboard: !this.state.isDashboard });
        }
        else {
            this.setState({ isDashboard: this.state.isDashboard });
        }
    }
    updateSelectedCompany = (e) => {

        const selectedCompanyName = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
        const selectedCompanyCode = e.target.value;
        const company = { companyCode: e.target.value, companyName: selectedCompanyName };
        this.props.actions.FetchUserPermissionsData([]);
        const allCoordinator = {
            allCoOrdinator: false
        };
        const allUserTaskandallCoOrdinatorSearch={ //changes happened For home dashboard my task and my search count not refreshing
            myTaskStatus:false,
            mySearchStatus:false
        };
        this.props.actions.ToggleAllCoordinator(allCoordinator);
        this.props.actions.MyTaskPropertyChange(allUserTaskandallCoOrdinatorSearch);
        this.props.actions.MySearchPropertyChange(allUserTaskandallCoOrdinatorSearch);
        this.props.actions.UpdateSelectedCompany(company);
        this.props.actions.UserMenu().then(x => {
            this.props.actions.ClearDashboardReducer();
            this.props.actions.Dashboardrefresh(this.props.history.location.pathname);
            this.props.actions.FetchDashboardCount();    
        });
        this.props.actions.UserType();
        this.props.actions.UpdateMasterCurrency();
        this.props.actions.ClearMyTaskAssignUser(); //D908 (#2Item 16-07-2020)
    }

    render() {
        //let companyDetails = this.props.companyList;
        // if (Array.isArray(companyDetails) && (this.props.companyList).length > 0) {
        //     companyDetails = (this.props.companyList).sort((a, b) => {
        //         return (a.companyName < b.companyName) ? -1 : (a.companyName > b.companyName) ? 1 : 0;
        //     });
        // }     
        let companyDetails = this.props.userRoleCompanyList;
        if (Array.isArray(companyDetails) && (this.props.userRoleCompanyList).length > 0) {
            companyDetails = (this.props.userRoleCompanyList).sort((a, b) => {
                return (a.companyName < b.companyName) ? -1 : (a.companyName > b.companyName) ? 1 : 0;
            });
        }
        const selectedCompanyCode = this.props.company ? this.props.company : this.props.selectedCompany;
        return (
            <Fragment>
                <select className="browser-default" onChange={(e) => this.updateSelectedCompany(e)} disabled={this.state.isDashboard}>
                    {
                        Array.isArray(companyDetails) ? (
                            (companyDetails).map(
                                (iteratedAttraction, i) =>
                                    <option className='attactions'
                                        value={iteratedAttraction.companyCode}
                                        selected={iteratedAttraction.companyCode === selectedCompanyCode ? true : false}
                                        key={iteratedAttraction.companyCode}
                                        name={iteratedAttraction.companyName}>
                                        {iteratedAttraction.companyName}
                                    </option>
                            )) : ''
                    }
                </select>
            </Fragment >
        );
    }
}

export default CompanyList;