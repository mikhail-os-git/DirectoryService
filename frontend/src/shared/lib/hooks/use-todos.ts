import { ICartItem, CartItemTypes, CART_ITEM_TYPES } from "@/shared/ui/card-item/CartItem.model";
import { useState } from "react";

const initialCarts: ICartItem[] = [
  { id: 1, text: 'Everything went perfectly!', type: 'success' },
  { id: 2, text: 'Some information for you.', type: 'info' },
  { id: 3, text: 'Proceed with caution.', type: 'warning' },
  { id: 4, text: 'An issue occurred.', type: 'error' },
  { id: 5, text: 'A notification for you.', type: 'default' },
];

export default function useTodos() {

  const [items, setItems] = useState<ICartItem[]>(initialCarts);

  const addItem = (text: string) => {
    if(text === "") return;
    
    const max: number = items.reduce((max, item) => (item.id > max ? item.id : max), 0); 
    const newItem: ICartItem = { id: max + 1, text, type: 'default' };
    setItems([...items, newItem])
  }

  const removeItem = (id: number) => {
    setItems((prev) => prev.filter((item) => item.id !== id));
  };

  const changeItem = (id: number, type: CartItemTypes) => {
    setItems((prev) => 
      prev.map(item => (item.id === id? {...item, type} : item))
    )
  }


  const itemsState: Record<CartItemTypes, number> = items.reduce(
  (acc, t) => {
    acc[t.type] += 1;
    return acc;
  },
  Object.fromEntries(CART_ITEM_TYPES.map((t) => [t, 0])) as Record<CartItemTypes, number>
);

    return { items, itemsState, addItem, removeItem, changeItem};
}