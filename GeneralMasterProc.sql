Alter Procedure Usp_GetMaster_Mateen
(
	@KeyName varchar(20)
)
As
Begin
	if (@KeyName = 'DiscountType')
	Begin
		Select top 10 Pid,DiscountType,Sequence from M_DiscountType
	End
	
	if(@KeyName = 'HumanResource')
	Begin
		select top 10 m_HumanResource.pid,m_HumanResource.id,m_HumanResource.Firstname,m_HumanResource.LastName, 
					  m_HumanResource.category,m_HumanResource.doj,m_HumanResource.Mobileno,m_HumanResource.Email, 
                      M_ResourceType.resourcetype,M_Designation.Designation
                      from m_HumanResource 
                      inner join M_ResourceType on m_HumanResource.ResourceTypeXid = M_ResourceType.pid 
                      inner join M_Designation on m_HumanResource.DesignationXid = m_Designation.pid 
	End
	
	if(@KeyName = 'Airport')
	Begin
	Select m_airport.pid,m_airport.code,m_airport.airport,m_country.Country,m_city.City,m_airport.lastedit from m_airport 
                                       inner join m_country on m_airport.countryxid = m_country.pid 
                                       inner join m_city on m_airport.cityxid = m_city.pid
	End
   
    if(@KeyName = 'ClientChain')
	Begin
	Select top 10  pid,code,clientchain,LastEdit from M_ClientChain
	End	
	
	if(@KeyName = 'LogisticPickupType')
	Begin
	select top 10 pid,pickuptype,showbookingengine,arrivalpoint,lastedit from M_LogisticPickupType
	End	
	
	if(@KeyName = 'PaymentType')
	Begin
	select top 10 pid,PaymentType,nominalcode,lastedit from M_paymentType
	End	
	
	if(@KeyName = 'PaymentSchedules')
	Begin
	select top 10 pid,PaymentSchedule,lastedit from M_PaymentSchedules
	End	
	
	if(@KeyName = 'Nationality')
	Begin
	select top 10 pid,nationality,lastedit from M_nationality
	End	
	
	if(@KeyName = 'MealPlan')
	Begin
	select top 10 pid,mealplan,lastedit from M_Mealplan
	End	
	
	if(@KeyName = 'Market')
	Begin
		select top 10 pid,market,lastedit from M_market
	End	
	
	if(@KeyName = 'Language')
	Begin
	select top 10 pid,code,languagename,lastedit from M_language
	End	
	
	if(@KeyName = 'InspectionCriteria')
	Begin
	select top 10 pid,inspectioncriteria,lastedit from M_InspectionCriteria
	End	
	
	if(@KeyName = 'HotelChain')
	Begin
	select top 10 pid,code,HotelChains from M_hotelchain
	End

	if(@KeyName = 'Standard')
	Begin
	select top 10 pid,standard from M_standard
	End

	if(@KeyName = 'HolidayType')
	Begin
	select top 10 pid,code,holidaytype from M_holidaytype
	End

	if(@KeyName = 'HolidayDuration')
	Begin
	select top 10 pid,code,holidayduration,lastedit from M_holidayduration
	End

	if(@KeyName = 'FinancialYear')
		Begin
		select top 10 pid,code,financialyear,fromdate,todate,lastedit from M_financialyear
		End

	if(@KeyName = 'Facility')
		Begin
		select top 10 pid,code,facility,belongsto,lastedit from M_Facility
		End

	if(@KeyName = 'TradeFairsTypes')
		Begin
		select top 10 pid,code,TradeFairsType,lastedit from M_TradeFairsTypes
		End

	if(@KeyName = 'Currency')
		Begin
		select top 10  pid,Code,Currency,CurrencySymbol,defaultcurrency,nominalcode,lastedit from m_currency
		End

	if(@KeyName = 'CardType')
		Begin
		Select top 10  pid,CardType,Length,CCChargesYN,CCCharges,CCChargeApplyTo,LastEdit from m_cardtype
		End

	if(@KeyName = 'BookingNote')
		Begin
		Select top 10  pid,code,NoteFor,Note from m_BookingNote
		End

	if(@KeyName = 'Bank')
		Begin
		Select top 10  pid,code,bank,BankBranch,GuichetCode,lastedit from m_bank
		End

	if(@KeyName = 'AddressType')
		Begin
		Select top 10  pid,addresstype,lastedit from m_addresstype
		End

	if(@KeyName = 'Activity')
		Begin
		Select top 10 pid,code,activity,lastedit from m_activity
		End

	if(@KeyName = 'RoomType')
		Begin
		select top 10 Pid, roomtype,isnull(MaxNoPpl,0) as MaxNoPpl,LastEdit  from m_roomtype
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



