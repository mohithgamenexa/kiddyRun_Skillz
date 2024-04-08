//
//  GeoComplyClientProtocol.h
//  GCSDKDomain
//
//  Created by Logan on 12/23/19.
//  Copyright © 2019 GeoComply. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@protocol GCClientDelegate <NSObject>

/**
 Tells the delegate a geolocation request is success.
 
 @param data Encrypted geolocation data returned from GeoComply Server. For decryption, use
 provided AES KEY & IV.
 */
- (void)didGeolocationAvailable:(NSString* _Nonnull)data;

/**
 Tells the delegate a geolocation request is failed.
 
 @param error A GCError object.
 
 @see GCError
 */
- (void)didGeolocationFailed:(GCError* _Nonnull)error;

@optional

/**
 Tells the delegate that there's a log message from GeoComply iOS SDK.
 
 @param message A log message from GeoComply iOS SDK.
 */
- (void)updateLog:(NSString* _Nullable)message;

/**
 Tells the delegate that Location Service is disabled and whether GeoComply
 iOS SDK should cancel current geolocation request.
 
 @return A determination flag whether GeoComply iOS SDK should cancel current
 geolocation request. Default is NO.
 
 @warning If this protocol is not implemented. GeoComply iOS SDK will still send
 current geolocation request to GeoComply Server for location determination. This
 is default behavior for GeoComply iOS SDK.
 */
- (BOOL)didLocationServiceDisabled;

/**
 Tells the delegate that GeoComply iOS SDK has stopped updating indoor location
 completely.
 
 @param info The detailed information.
 */
- (void)didStopUpdating:(NSDictionary* _Nullable)info;

/**
 Telss the delegate that GeoCompoly iOS SDK is asking to display a message to turn
 on Bluetooth.
 
 @param message The message for app to display.
 */
- (void)didTurnOffBluetooth:(NSString* _Nullable)message;

@end

/**
 The GeoComplyClient object manages all geolocation request call
 */
@protocol GeoComplyClientProtocol <NSObject>

@required
- (NSString * _Nullable)userId;

- (void)setUserId:(NSString * _Nullable)userId;

- (NSString * _Nullable)geolocationReason;

- (void)setGeolocationReason:(NSString * _Nullable)geolocationReason;

- (NSString * _Nullable)userPhoneNumber;

- (void)setUserPhoneNumber:(NSString * _Nullable)userPhoneNumber;

- (GCCustomFields * _Nullable)customFields;

- (id<GCClientDelegate> _Nullable)delegate;

- (void)setDelegate:(id<GCClientDelegate> _Nullable)delegate;

- (BOOL)setLicense:(NSString* _Nonnull)license error:(NSError* _Nullable __autoreleasing* _Nullable)errorPtr;

- (BOOL)requestGeolocation:(NSError* _Nullable __autoreleasing* _Nullable)error;

- (BOOL)requestGeolocation:(NSError* _Nullable __autoreleasing* _Nullable)error timeout:(NSInteger)seconds;

+ (id<GeoComplyClientProtocol> _Nonnull)instance;

- (void)handleMemoryWarning;

- (BOOL)startUpdating:(NSError* _Nullable __autoreleasing* _Nullable)error;

- (void)stopUpdating;

- (BOOL)isUpdating;

- (NSString* _Nullable)currentRequestUUID;

- (NSString * _Nonnull)version;

+ (void)handleLaunchOptions:(NSDictionary* _Nullable)launchOptions;

@optional
+ (void)touchesBegan:(NSSet* _Nonnull)touches withEvent:(UIEvent* _Nonnull)event onView:(UIView* _Nonnull)view;

- (void)setUserSessionID:(NSString* _Nullable)userSessionID;

- (NSString *_Nullable)currentUserSessionID;

- (void)invalidateUserSession;

- (BOOL)isGeolocationInProgress;

- (void)cancelCurrentGeolocationReason:(GCCancelReason)reason
                              details:(NSString* _Nullable)details
                           completion:(void(^ _Nonnull)(BOOL success, NSString* _Nullable description))completion;

- (void)setGameCode:(GCGameCode)gameCode;

- (void)setReasonCode:(GCReasonCode)reasonCode;

- (GCReasonCode)reasonCode;

- (GCGameCode)gameCode;
@end

@protocol GeoComplyMultipleSDKProtocol <NSObject>
- (void)useSDKVersion:(NSString * _Nonnull)version error:(NSError* _Nullable __autoreleasing* _Nullable)error;

- (NSString * _Nonnull)currentUsingSDKVersion;

+ (void)handleLaunchOptions:(NSDictionary* _Nullable)launchOptions sdkVersion:(NSString* _Nonnull)sdkVersion;

+ (void)touchesBegan:(NSSet* _Nonnull)touches withEvent:(UIEvent* _Nonnull)event onView:(UIView* _Nonnull)view sdkVersion:(NSString* _Nonnull)sdkVersion;
@end

