



Tables

select * from UserGroupRights1_Mateen

Pid	UserGroupXid	ModuleXid	PageName	LinkName	RightHeader	AllowYN	NotesXid	LastEdit	LastEditByXid	CompanyXid
1	90	1	DisplayDiscountType	View	DiscountType	Y	NULL	2018-01-03 14:47:55.643	733	2
2	90	1	DisplayRoomType	View	RoomType	Y	NULL	2018-01-11 13:22:54.027	733	2
3	90	1	DisplayAirport	View	Airport	Y	NULL	2018-01-13 08:10:58.483	733	2
4	90	1	DisplayHumanResource	View	HumanResource	Y	NULL	2018-01-20 13:48:44.243	733	2
5	90	1	DisplayDiscountType	Add	ActionDiscountType	Y	NULL	2018-02-14 08:57:19.107	733	2
6	90	1	DisplayDiscountType	Edit	EditDiscountType	Y	NULL	2018-02-14 08:57:19.110	733	2
7	90	1	DisplayDiscountType	Delete	DeleteDiscountType	Y	NULL	2018-02-14 08:57:19.117	733	2



proc used

sp_helptext Usp_GetDiscountTypeById_Mateen
sp_helptext Usp_GetMaster_Mateen
sp_helptext Usp_GetNotesById_Mateen
sp_helptext Usp_InsertModifyDiscountType_Mateen
sp_helptext Usp_NotesChanges_Mateen


Text
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE procedure Usp_GetDiscountTypeById_Mateen
(
@li_pid int,
@lv_action varchar(1)
)
as
Declare 
@NotesXid int
Begin
	if (@lv_action = 'E')
		begin
			Select DiscountType,Sequence,LastEditbyXid,Companyxid,isnull(NotesXid,-1) as NotesXid
			from M_DiscountType
			Where pid = @li_pid
		end
	if (@lv_action = 'D')
		begin
		Select @NotesXid=NotesXid
			from M_DiscountType
			Where pid = @li_pid
		
			if (@NotesXid != null or @NotesXid != -1)
				begin
				Delete from M_Notes where pid = @NotesXid
				end

			Delete from M_DiscountType
			Where pid = @li_pid
		end
End

Text
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE Procedure Usp_GetMaster_Mateen
(
	@lv_KeyName varchar(20),
	@li_SearchPid int	
)
As
Begin
	if (@lv_KeyName = 'DiscountType')
		Begin
			if(@li_SearchPid = 0)
			Begin
				Select  Pid,DiscountType,Sequence,lastEdit from M_DiscountType 
			End
			if (@li_SearchPid > 0)
			Begin
				Select  Pid,DiscountType,Sequence,lastEdit from M_DiscountType where pid = @li_SearchPid
			End
		End
	
	if(@lv_KeyName = 'HumanResource')
	Begin
		select		  m_HumanResource.pid,m_HumanResource.id,m_HumanResource.Firstname,m_HumanResource.LastName, 
					  m_HumanResource.category,m_HumanResource.doj,m_HumanResource.Mobileno,m_HumanResource.Email, 
                      M_ResourceType.resourcetype,M_Designation.Designation
                      from m_HumanResource 
                      inner join M_ResourceType on m_HumanResource.ResourceTypeXid = M_ResourceType.pid 
                      inner join M_Designation on m_HumanResource.DesignationXid = m_Designation.pid 
	End
	
	if(@lv_KeyName = 'Airport')
	Begin
	Select m_airport.pid,m_airport.code,m_airport.airport,m_country.Country,m_city.City,m_airport.lastedit from m_airport 
                                       inner join m_country on m_airport.countryxid = m_country.pid 
                                       inner join m_city on m_airport.cityxid = m_city.pid
	End
   
    if(@lv_KeyName = 'ClientChain')
	Begin
	Select top 10  pid,code,clientchain,LastEdit from M_ClientChain
	End	
	
	if(@lv_KeyName = 'LogisticPickupType')
	Begin
	select top 10 pid,pickuptype,showbookingengine,arrivalpoint,lastedit from M_LogisticPickupType
	End	
	
	if(@lv_KeyName = 'PaymentType')
	Begin
	select top 10 pid,PaymentType,nominalcode,lastedit from M_paymentType
	End	
	
	if(@lv_KeyName = 'PaymentSchedules')
	Begin
	select top 10 pid,PaymentSchedule,lastedit from M_PaymentSchedules
	End	
	
	if(@lv_KeyName = 'Nationality')
	Begin
	select top 10 pid,nationality,lastedit from M_nationality
	End	
	
	if(@lv_KeyName = 'MealPlan')
	Begin
	select top 10 pid,mealplan,lastedit from M_Mealplan
	End	
	
	if(@lv_KeyName = 'Market')
	Begin
		select top 10 pid,market,lastedit from M_market
	End	
	
	if(@lv_KeyName = 'Language')
	Begin
	select top 10 pid,code,languagename,lastedit from M_language
	End	
	
	if(@lv_KeyName = 'InspectionCriteria')
	Begin
	select top 10 pid,inspectioncriteria,lastedit from M_InspectionCriteria
	End	
	
	if(@lv_KeyName = 'HotelChain')
	Begin
	select top 10 pid,code,HotelChains from M_hotelchain
	End

	if(@lv_KeyName = 'Standard')
	Begin
	select top 10 pid,standard from M_standard
	End

	if(@lv_KeyName = 'HolidayType')
	Begin
	select top 10 pid,code,holidaytype from M_holidaytype
	End

	if(@lv_KeyName = 'HolidayDuration')
	Begin
	select top 10 pid,code,holidayduration,lastedit from M_holidayduration
	End

	if(@lv_KeyName = 'FinancialYear')
		Begin
		select top 10 pid,code,financialyear,fromdate,todate,lastedit from M_financialyear
		End

	if(@lv_KeyName = 'Facility')
		Begin
		select top 10 pid,code,facility,belongsto,lastedit from M_Facility
		End

	if(@lv_KeyName = 'TradeFairsTypes')
		Begin
		select top 10 pid,code,TradeFairsType,lastedit from M_TradeFairsTypes
		End

	if(@lv_KeyName = 'Currency')
		Begin
		select top 10  pid,Code,Currency,CurrencySymbol,defaultcurrency,nominalcode,lastedit from m_currency
		End

	if(@lv_KeyName = 'CardType')
		Begin
		Select top 10  pid,CardType,Length,CCChargesYN,CCCharges,CCChargeApplyTo,LastEdit from m_cardtype
		End

	if(@lv_KeyName = 'BookingNote')
		Begin
		Select top 10  pid,code,NoteFor,Note from m_BookingNote
		End

	if(@lv_KeyName = 'Bank')
		Begin
		Select top 10  pid,code,bank,BankBranch,GuichetCode,lastedit from m_bank
		End

	if(@lv_KeyName = 'AddressType')
		Begin
		Select top 10  pid,addresstype,lastedit from m_addresstype
		End

	if(@lv_KeyName = 'Activity')
		Begin
		Select top 10 pid,code,activity,lastedit from m_activity
		End

	if(@lv_KeyName = 'RoomType')
		Begin
		select top 10 Pid, roomtype,isnull(MaxNoPpl,0) as MaxNoPpl,LastEdit  from m_roomtype
		End

	if(@lv_KeyName = 'Season')
		Begin
		select top 10 m_season.Pid,m_season.code,m_season.season,m_season.FromDate,m_season.ToDate,m_financialyear.FinancialYear,m_season.lastEdit  from m_season inner join
					  m_financialyear on  	m_season.FinancialYearXid  = m_financialyear.pid
		End



--CrmPriority
--PropertyType
--Reason
--ReportingState
--Season
--Source
--Status
--Supplement
--SupplementType
--Tax
--Title
--Company
--Department
--Designation
--DMCSystemConfiguration
--ImageLibrary
--Depot
--ContractingGroup
--TblTariff
--TblTariffMarkets
--Client
--Airline

--ResourceType
--ResourceVehicleDtls
--LogisticPickupArea
--LogisticVehicleType
--HumanResource
--LogisticJourneyTimes


End







Text
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE procedure Usp_GetNotesById_Mateen
(
@li_pid int,
@lv_action varchar(1)
)
as

Begin
Declare
@NotesDesc varchar(200)

	if(@lv_action = 'E')
		Begin
			Select @NotesDesc=Notes
			from M_Notes
			Where pid = @li_pid

			select @NotesDesc
		End


End

Text
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE Procedure Usp_InsertModifyDiscountType_Mateen
(
	@li_Pid int,
	@lv_DiscountTypeName varchar(20),
	@li_Sequence int,
	@li_NotesXid int,
	@li_LastEditByXid int,
	@li_Companyxid int,
	@lc_Action varchar(1)
)
as
Begin

	if (@lc_Action = 'A')
	  Begin
			Insert into M_DiscountType (DiscountType,NotesXid,LastEdit,LastEditByXid,CompanyXid,Sequence)
			values
			(
			@lv_DiscountTypeName,@li_NotesXid,getdate(),@li_LastEditByXid,@li_Companyxid,@li_Sequence
			)
	  End	

	if (@lc_Action = 'E')
	  Begin
		Update M_DiscountType
		set DiscountType=@lv_DiscountTypeName,
			NotesXid=@li_NotesXid,
			LastEdit=getdate(),
			LastEditByXid=@li_LastEditByXid,
			CompanyXid=@li_Companyxid,
			Sequence=@li_Sequence
		where pid= @li_pid
	  End

End


Text
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE Procedure Usp_NotesChanges_Mateen
(
@li_pid int,
@lv_notes varchar(16),
@li_lastedit int,
@li_companyxid int,
@lc_action varchar(1)
)
As
Begin

Declare 
@li_maxpid int 

	if (@lc_action = 'A')
	 begin
		Insert Into M_Notes (Notes,EntityType,EntityXid,LastEdit,LastEditByXid,CompanyXid,HTMLYN,privateyn) 
					 values (@lv_notes,null,null,getdate(),@li_lastedit,@li_companyxid,null,null)
		select @li_maxpid = max(pid) from m_notes
		select @li_maxpid as pid
	 end

	if (@lc_action = 'E')
	 begin
		Update M_Notes
		set Notes = @lv_notes,
			LastEdit = getdate(),
			LastEditByXid = @li_lastedit,
			CompanyXid = @li_companyxid	
		where pid = @li_pid
		select @li_pid as pid
	 end
End





