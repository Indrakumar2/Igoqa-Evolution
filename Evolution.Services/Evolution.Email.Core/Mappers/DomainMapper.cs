using AutoMapper;
using Evolution.Email.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Email.Domain.Models;

namespace Evolution.Email.Core.Mappers
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            #region Emial

            CreateMap<DbModel.Email, DomainModel.EmailQueueMessage>()
            .ForMember(source => source.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FromAddresses, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.FromEmail) ? src.FromEmail.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(x => new EmailAddress() { Address = x }).ToList() : null))
            .ForMember(dest => dest.ToAddresses, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.ToEmail) ? src.ToEmail.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(x => new EmailAddress() { Address = x }).ToList() : null))
            .ForMember(dest => dest.BccAddresses, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.BccEmail) ? src.BccEmail.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(x => new EmailAddress() { Address = x }).ToList() : null))
            .ForMember(dest => dest.CcAddresses, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.CcEmail) ? src.CcEmail.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(x => new EmailAddress() { Address = x }).ToList() : null))
            .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.BodyContent))
            .ForMember(dest => dest.EmailStatus, opt => opt.MapFrom(src => src.EmailStatus))
            .ForMember(dest => dest.StatusReason, opt => opt.MapFrom(src => src.StatusReason))
            .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
            .ForMember(dest => dest.LastAttemptOn, opt => opt.MapFrom(src => src.LastAttemptOn))
            .ForMember(dest => dest.RetryCount, opt => opt.MapFrom(src => src.RetryCount))
            .ForMember(dest => dest.ModuleCode, opt => opt.MapFrom(src => src.ModuleCode))
            .ForMember(dest => dest.ModuleEmailRefCode, opt => opt.MapFrom(src => src.ModuleEmailRefCode))
            .ForMember(dest => dest.EmailType, opt => opt.MapFrom(src => src.EmailType))
            .ForMember(dest => dest.BodyPlaceHolderAndValue, opt => opt.MapFrom(src => src.BodyPlaceHolderAndValue))
            .ForMember(dest => dest.IsMailSendAsGroup, opt => opt.MapFrom(src => Convert.ToBoolean(src.IsMailSendAsGroup)))
            .ForMember(dest => dest.IsContentEncrypt, opt => opt.MapFrom(src => src.IsMailContentEncrypted))
            .ForMember(dest => dest.PrivateKey, opt => opt.MapFrom(src => src.PrivateKey))
            .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Attachment) ? src.Attachment.Split('|', StringSplitOptions.RemoveEmptyEntries).Select(x => new Attachment() { UniqueKey = x }).ToList() : null))

            .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModel.EmailQueueMessage, DbModel.Email>()
            .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.Id : 0) : src.Id)))
            .ForMember(dest => dest.FromEmail, opt => opt.ResolveUsing(src => JoinEmailAddress(src.FromAddresses)))
            .ForMember(dest => dest.ToEmail, opt => opt.ResolveUsing(src =>JoinEmailAddress( src.ToAddresses)))
            .ForMember(dest => dest.BccEmail, opt => opt.ResolveUsing(src => JoinEmailAddress(src.BccAddresses)))
            .ForMember(dest => dest.CcEmail, opt => opt.ResolveUsing(src => JoinEmailAddress(src.CcAddresses)))
            .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
            .ForMember(dest => dest.BodyContent, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.EmailStatus, opt => opt.MapFrom(src => src.EmailStatus))
            .ForMember(dest => dest.StatusReason, opt => opt.MapFrom(src => src.StatusReason))
            .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
            .ForMember(dest => dest.LastAttemptOn, opt => opt.MapFrom(src => src.LastAttemptOn))
            .ForMember(dest => dest.RetryCount, opt => opt.MapFrom(src => src.RetryCount))
            .ForMember(dest => dest.ModuleCode, opt => opt.MapFrom(src => src.ModuleCode))
            .ForMember(dest => dest.ModuleEmailRefCode, opt => opt.MapFrom(src => src.ModuleEmailRefCode))
            .ForMember(dest => dest.EmailType, opt => opt.MapFrom(src => src.EmailType))
            .ForMember(dest => dest.BodyPlaceHolderAndValue, opt => opt.MapFrom(src => src.BodyPlaceHolderAndValue))
            .ForMember(dest => dest.IsMailSendAsGroup, opt => opt.MapFrom(src => src.IsMailSendAsGroup))
            .ForMember(dest => dest.IsMailContentEncrypted, opt => opt.MapFrom(src => src.IsContentEncrypt))
            .ForMember(dest => dest.PrivateKey, opt => opt.MapFrom(src => src.PrivateKey))
            .ForMember(dest => dest.Attachment, opt => opt.MapFrom(src => JoinAttachmentFileName(src.Attachments)))
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token))

            .ForAllOtherMembers(src => src.Ignore());

            #endregion 
        }

        private string JoinEmailAddress(IList<EmailAddress> emails)
        {
            string result = string.Empty;
            emails?.ToList().ForEach(email =>
            {
                if (string.IsNullOrEmpty(result))
                    result = email.Address;
                else
                    result = string.Format("{0};{1}",result, email.Address);
            });

            return result;
        }

        private string JoinAttachmentFileName(IList<Attachment> attachments)
        {
            string result = string.Empty;
            attachments?.ToList().ForEach(attachment =>
            {
                if (string.IsNullOrEmpty(result))
                    result = attachment.UniqueKey;
                else
                    result = string.Format("{0}|{1}", result, attachment.UniqueKey);
            });

            return result;
        }
    }
}
