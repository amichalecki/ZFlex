using ZFlex.Models;
using ZFlex.Models.AffectationsByUserAndDates;
using ZFlex.Models.CreateAffectation;
using ZFlex.Models.DeleteAffectations;
using ZFlex.Models.MapView;

namespace ZFlex.Repositories
{
    public class GqlQueriesRepository
    {
        internal static GqlQuery<CreateAffectationVariables> CreateAffectationQuery(Desk desk, Guid userId, DateOnly date, ParkingSpot? parkingSpot = null)
            => new()
            {
                OperationName = "createAffectation",
                Query = @"mutation createAffectation($data: CreateSimpleAffectationInput!) {
                          createAffectation(data: $data) {
                            id
                            userId
                            guestId
                            deskId
                            spaceId
                            services {
                              id
                              type
                              parkingSpotId
                              __typename
                            }
                            __typename
                          }
                        }",
                Variables = new(desk, userId, date, parkingSpot)
            };

        internal static GqlQuery<DeleteAffectationsVariables> DeleteAffectationsQuery(IEnumerable<Guid> affectationsIds)
            => new()
            {
                OperationName = "deleteAffectations",
                Query = @"mutation deleteAffectations($params: [AffectationDeletionInput!]!) {
                          deleteAffectations(params: $params) {
                            success
                            deletedServicesIds
                            __typename
                          }
                        }",
                Variables = new(affectationsIds)
            };

        internal static GqlQuery<AffectationsByUserAndDatesVariables> AffectationsByUserAndDatesQuery(Guid userId, IEnumerable<DateOnly> dates)
            => new()
            {
                OperationName = "affectationsByUserAndDates",
                Query = @"query affectationsByUserAndDates($userId: UserIdType!, $affectationsFilter: GetAffectationsFilter!) {
                          user(idV2: $userId) {
                            id
                            firstEditableDayMomentOfCaller(isCalledFromPlanning: true) {
                              date
                              moment
                              __typename
                            }
                            affectations(affectationFilter: $affectationsFilter) {
                              id
                              eventId
                              date
                              moment
                              active
                              userId
                              createdById
                              desk {
                                id
                                name
                                coordinates
                                __typename
                              }
                              createdBy {
                                id
                                fullName
                                __typename
                              }
                              space {
                                ...spaceForAffectationPlaning
                                __typename
                              }
                              type
                              description
                              event {
                                id
                                eventParticipantTargets {
                                  polymorphicType
                                  data {
                                    ... on Team {
                                      id
                                      __typename
                                    }
                                    ... on User {
                                      id
                                      __typename
                                    }
                                    ... on Guest {
                                      id
                                      __typename
                                    }
                                    __typename
                                  }
                                  __typename
                                }
                                manageable
                                userCount
                                __typename
                              }
                              recurrence {
                                id
                                period
                                frequencyType
                                __typename
                              }
                              guestAffectations {
                                id
                                eventId
                                __typename
                              }
                              guestAffectationsCount
                              services {
                                ...serviceShortInfo
                                __typename
                              }
                              suggestionCount
                              __typename
                            }
                            suggestions(affectationFilter: $affectationsFilter) {
                              id
                              date
                              moment
                              suggestionNotification {
                                id
                                __typename
                              }
                              guestAffectationsCount
                              __typename
                            }
                            canSeePresenceRules
                            __typename
                          }
                        }

                        fragment spaceForAffectationPlaning on Space {
                          id
                          name
                          inheritedName
                          serviceType
                          desksEnabled
                          parent {
                            id
                            hasMap
                            __typename
                          }
                          __typename
                        }

                        fragment serviceShortInfo on ServiceReservation {
                          id
                          type
                          spaceId
                          userId
                          parkingSpotId
                          timeSlotId
                          space {
                            id
                            name
                            inheritedName
                            __typename
                          }
                          parkingSpot {
                            ...parkingSpotShortInfo
                            __typename
                          }
                          __typename
                        }

                        fragment parkingSpotShortInfo on ParkingSpot {
                          id
                          name
                          __typename
                        }",
                Variables = new(userId, dates)
            };

        internal static GqlQuery<MapViewVariables> MapViewQuery(Guid spaceId, Guid userId, DateOnly date)
            => new()
            {
                OperationName = "mapView",
                Query = @"query mapView($spaceId: ID!, $affectationData: SpaceAffectationDataInput!, $spaceAvailibilityData: SpaceAvailibilityInput!, $mainUserIdV2: UserIdType!, $serviceTypesToExclude: [ServiceTypeEnum!], $includeLeavesIds: [String!], $skipOfficeEquipment: Boolean! = true, $skipAffectations: Boolean! = false, $date: String!) {
                  space(id: $spaceId) {
                    id
                    name
                    spaceLeaves(
                      exclusive: true
                      serviceTypesToExclude: $serviceTypesToExclude
                      includeLeavesIds: $includeLeavesIds
                    ) {
                      ...MapViewSpaceLeave
                      __typename
                    }
                    __typename
                  }
                }

                fragment MapViewSpaceLeave on Space {
                  id
                  name
                  inheritedName
                  serviceType
                  spaceType
                  isCommonSpace
                  affectationCount(affectationData: $affectationData)
                  realCapacity
                  desks {
                    ...MapViewDesk
                    __typename
                  }
                  parkingSpots {
                    ...MapViewParkingSpot
                    __typename
                  }
                  affectations(affectationData: $affectationData) @skip(if: $skipAffectations) {
                    ...MapViewAffectation
                    __typename
                  }
                  spaceAvailibility(data: $spaceAvailibilityData) {
                    isAvailable
                    bookablePlaces
                    failureMessage
                    __typename
                  }
                  __typename
                }

                fragment MapViewDesk on Desk {
                  id
                  name
                  isLocked
                  isLockedAt(date: $date)
                  spaceId
                  name
                  exclusiveUser {
                    id
                    firstName
                    lastName
                    pictureUrl
                    __typename
                  }
                  exclusiveUserAffectationMoment(affectationData: $affectationData)
                  officeEquipments @skip(if: $skipOfficeEquipment) {
                    id
                    name
                    __typename
                  }
                  __typename
                }

                fragment MapViewParkingSpot on ParkingSpot {
                  id
                  name
                  isLocked
                  isLockedAt(date: $date)
                  spaceId
                  name
                  exclusiveUserParkingSpotAffectations(affectationData: $affectationData)
                  availability(affectationData: $affectationData, mainUserIdV2: $mainUserIdV2)
                  __typename
                }

                fragment MapViewAffectation on Affectation {
                  id
                  createdBy {
                    fullName
                    __typename
                  }
                  date
                  deskId
                  spaceId
                  moment
                  userId
                  guestId
                  description
                  type
                  active
                  user {
                    id
                    firstName
                    lastName
                    pictureUrl
                    firstEditableDayMomentOfCaller {
                      date
                      moment
                      __typename
                    }
                    __typename
                  }
                  guest {
                    id
                    description
                    __typename
                  }
                  services {
                    id
                    type
                    parkingSpotId
                    __typename
                  }
                  __typename
                }",
                Variables = new(spaceId, userId, date)
            };
    }
}
