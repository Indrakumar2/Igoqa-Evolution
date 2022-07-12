import InputWithPopUpSearch from './inputWithPopUpSearch';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { bindActionCreators } from 'redux';

const mapStateToProps = (state) => {
    return {     
           countryMasterData: state.masterDataReducer.countryMasterData        
    };
};

export default connect(mapStateToProps)(withRouter(InputWithPopUpSearch));