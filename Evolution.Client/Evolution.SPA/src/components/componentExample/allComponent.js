import React, { Component, Fragment } from 'react';
import MaterializeComponent from 'materialize-css';
import CustomInput from '../../common/baseComponents/inputControlls';
import { getlocalizeData } from '../../utils/commonUtils';
import moment from 'moment';
const localConstant = getlocalizeData();
class AllComponent extends Component {
    constructor(props) {
        super(props);
        this.state = {
            defaultValue: 'a',
            startDate: '',
            passwordToggle: false,
            userName: '',
            userPassword: '',
            userNameError: false,
            userPasswordError: false
        };
    }

    passwordToggleHandler = (event) => {
        this.setState({ passwordToggle: !this.state.passwordToggle });
    }
    handleCredential = (e) => {
        this.setState({
            [e.target.name]: e.target.value
        });
    }
    onRadioChange = (e) => {
        const value = e.target.value;
        this.setState({ defaultValue: value });
    }

    oncheckBoxChange = (e) => {
        const value = e.target.value;
        this.setState({ defaultValue: value });
    }
    handleChange = (date) => {
        this.setState({
            startDate: date
        });
    }
    render() {
        const companyDetails = [
            { value: 'Rock' },
            { value: 'Paper' },
            { value: 'Scissors' }
        ];
        const optionGroup = [
            {
                label: 'Rajasthan',
                options: [ {
                    id: 1,
                    text: 'Jaipur'
                },
                {
                    id: 2,
                    text: 'Ajmer'
                },
                {
                    id: 3,
                    text: 'Jodhpur'
                } ]
            },

            {
                label: 'Gujarat',
                options: [ {
                    id: 4,
                    text: 'Surat'
                },
                {
                    id: 5,
                    text: 'Vadodara'
                },
                {
                    id: 6,
                    text: 'Ahmedabad'
                } ]
            }
        ];
        const radioArray = [
            { value: "a", label: "Option 1" },
            { value: "b", label: "Option 2" },
            { value: "c", label: "Option 3" },
            { value: "d", label: "Option 4", disabled: true },
            { value: "e", label: "Option 5" }
        ];
        const checkBoxArray = [
            { value: "a", label: "Checkbox 1" },
            { value: "b", label: "Checkbox 2" },
            { value: "c", label: "Checkbox 3" },
            { value: "d", label: "Checkbox 4", disabled: true },
            { value: "e", label: "Checkbox 5" }
        ];
        return (
            <Fragment>
                <div className="col s6">
                    <CustomInput hasLabel={false}
                        type='text'
                        prefixIcon='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAALEwAACxMBAJqcGAAAAbFJREFUSIm11j1rFHEQBvBfLtgIWvhWyGVzveLrJ/D0G0hexELFVhTRDyH4CRSbICHGwlYQUtkp6VSsBAk2KhoiGCPRs/jvcutmd/Y88IFpbuZ5Zv5zM8NOiNHDPPo4goP575/wGit4hPctOjuQYQm/MGixbSyiO6r4PL6NIFy1Dcy0id8cQ7hsv3E9qryJ+BmXsCe3y/gSJNnxkixoy3ccqynoJDYbOBsq/8lSUP2dpifjbsB7WAT1xNNyPEhwKuBtI+tIve8EIm8D35vAN4m5jrRE/wt9+CAevROBQNSiAdY6huvfhAuB72IL9xD8bKlis+EVp/GjhbtFe4sG0lJdwd7cruLrCLw1eDZC4Lj2dBKHcS7o43M8wG4ckOb7Je5Lo9gLuPdgOidVs7/D2YBc4EweW7doU0XQYsX5AvtGEC+wH6sVjYVyQFc6UIUz+wfxAlmJvy61/i/MSqd2gFtjJLhteK7PNwXdKCV5rNTDANN4UhK/1kaYMWzXlnTKZ6Vp2ZVbD3NYNlzU9ajyKrrSPa+brrppWVDTc5hoSZTllfZxVH5b8BGvpM+WZfnG1uEPG8YCLranP3QAAAAASUVORK5CYII='
                        name='userName'
                        colSize='s12'
                        inputClass={this.state.userNameError ? "validate pl-3 invalid" : "validate pl-3"}
                        errorMsg={localConstant.validationMessage.INVALIED_USER_NAME}
                        onValueChange={this.handleCredential}
                    />

                    <CustomInput hasLabel={false}
                        type='password'
                        prefixIcon='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAALEwAACxMBAJqcGAAAAbFJREFUSIm11j1rFHEQBvBfLtgIWvhWyGVzveLrJ/D0G0hexELFVhTRDyH4CRSbICHGwlYQUtkp6VSsBAk2KhoiGCPRs/jvcutmd/Y88IFpbuZ5Zv5zM8NOiNHDPPo4goP575/wGit4hPctOjuQYQm/MGixbSyiO6r4PL6NIFy1Dcy0id8cQ7hsv3E9qryJ+BmXsCe3y/gSJNnxkixoy3ccqynoJDYbOBsq/8lSUP2dpifjbsB7WAT1xNNyPEhwKuBtI+tIve8EIm8D35vAN4m5jrRE/wt9+CAevROBQNSiAdY6huvfhAuB72IL9xD8bKlis+EVp/GjhbtFe4sG0lJdwd7cruLrCLw1eDZC4Lj2dBKHcS7o43M8wG4ckOb7Je5Lo9gLuPdgOidVs7/D2YBc4EweW7doU0XQYsX5AvtGEC+wH6sVjYVyQFc6UIUz+wfxAlmJvy61/i/MSqd2gFtjJLhteK7PNwXdKCV5rNTDANN4UhK/1kaYMWzXlnTKZ6Vp2ZVbD3NYNlzU9ajyKrrSPa+brrppWVDTc5hoSZTllfZxVH5b8BGvpM+WZfnG1uEPG8YCLranP3QAAAAASUVORK5CYII='
                        name='userName'
                        colSize='s12'
                        inputClass={this.state.userNameError ? "validate pl-3 invalid" : "validate pl-3"}
                        errorMsg={localConstant.validationMessage.INVALIED_USER_NAME}
                        onValueChange={this.handleCredential}
                        showPasswordToggle={this.passwordToggleHandler}
                    />
                    <CustomInput
                        hasLabel={true}
                        label="company Name"
                        divClassName="m12"
                        type='text'
                        inputName='Search'
                        placeholder='search..'
                        colSize='s12'
                        inputClass="customInputs searchtxtbox"

                    />
                    <CustomInput
                        hasLabel={false}
                        divClassName="m12"
                        type='text'
                        inputName='textBox'
                        placeholder='Empty Text Box..'
                        colSize='s12'
                        inputClass="customInputs searchtxtbox"

                    />
                    <CustomInput
                        hasLabel={false}
                        divClassName="m12"
                        type='textarea'
                        inputName='textBox'
                        placeholder='Empty TextArea Box..'
                        colSize='s12'
                        inputClass="customInputs"

                    />
                    <CustomInput
                        hasLabel={false}
                        divClassName='col'
                        label='select Box'
                        type='select'
                        onSelectChange={(e) => this.updateSelectedCompany(e)}
                        colSize='s12'
                        className="browser-default"
                        optionsList={companyDetails}
                        defaultValue={this.props.selectedCompany}
                        optionName='value'
                        optionValue="value"
                    />
                    <CustomInput
                        type='selectOptionGroup'
                        hasLabel={true}
                        label="State"
                        divClassName='col'
                        colSize='s12'
                        optionGroups={optionGroup}
                        optionGroupLabelKey='label'
                        optionKeyValue="text"
                        subOption="options"
                    />
                    <CustomInput
                        hasLabel={true}
                        label='Paymnet type'
                        labelClass='col s12'
                        type='checkbox'
                        checkBoxArray={checkBoxArray}
                        colSize='s12'
                        value='value'
                        name="label"
                        oncheckBoxChange={this.oncheckBoxChange}
                    />
                    <CustomInput
                        hasLabel={true}
                        label='Paymnet type'
                        labelClass='col s12'
                        type='radio'
                        optionsArray={radioArray}
                        colSize='s12'
                        value='value'
                        name="label"
                        defaultValue={this.state.defaultValue}
                        onRadioChange={this.onRadioChange}
                    />

                    <CustomInput
                        hasLabel={true}
                        isNonEditDateField={false}
                        label='Date of Birth'
                        labelClass='col s12'
                        colSize='s12'
                        type='date'
                        selectedDate={this.state.startDate}
                        onDateChange={this.handleChange}

                    />
                    <CustomInput
                        type='switch'
                        switchLabel="Default"
                        isSwitchLabel={true}
                        switchName="default"
                    />

                </div>

            </Fragment>
        );
    }
}
export default AllComponent;