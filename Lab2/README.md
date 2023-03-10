# Coupon sharer website

### Description:
The main goal of this project is to create a resource where everyone can share coupons they don't need or to find some coupons they need. As an example when you buy some keyboard set sometimes you may find some kind of a coupon for in-game bonuses and when you don't play this game as a good person you visit social medias and drop photos of this coupon. On this website people can exchange their coupons via much easier way.
[Link](https://www.figma.com/file/hVtEUirDKTP2HfESEKrzmQ/ShareTheCoupo?node-id=521%3A198&t=bWCF8sZIFBnIc086-0) to figma mockup. 

### Main functions are:
- Authorization.
- Upload, edit or delete your own coupon to the site.
- Search coupons.
- View other's profiles.
- Book coupons so they won't be shown to other's
- Check history of your claimed coupons.

### Data models:

- User:
    - [PK] UserID
    - Avatar
    - Nickname
    - Email
    - Password
    - KarmaPoints
    - IsAdmin
    
- Coupon:
    - [PK] CouponID
    - Title
    - Description
    - Code
    - Author
    - ClaimedBy
    - IsReusable
    - UsesLeft
    
- CategoryLink:
    - [FK] CouponID
    - [FK] CategoryID
    
- Category:
    - [PK] CategoryID
    - CategoryName
