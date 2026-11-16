import { CartItemTypes } from "../card-item/CartItem.model";

export interface CartSummaryProps extends React.ComponentProps<'div'>
{ 
  counts: Record<CartItemTypes, number>
}
