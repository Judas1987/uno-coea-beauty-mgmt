-- Set the schema
SET search_path TO master;

-- Insert initial service categories
INSERT INTO service_categories (title, description) VALUES
    ('Hair Care', 'All hair-related services including cuts, coloring, and styling'),
    ('Nail Care', 'Manicures, pedicures, and nail art services'),
    ('Facial Treatments', 'Facials, cleansing, and skin care treatments'),
    ('Massage', 'Various massage therapy services'),
    ('Makeup', 'Professional makeup application and consultations'),
    ('Waxing', 'Hair removal services for all body parts'),
    ('Body Treatments', 'Body wraps, scrubs, and other spa treatments'),
    ('Eyelash & Eyebrow', 'Lash extensions, tinting, and eyebrow shaping'); 