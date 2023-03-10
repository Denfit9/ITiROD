# Coupon sharer website

### Description:
The main goal of this project is to create a resource where everyone can share coupons they don't need or to find some coupons they need.
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
